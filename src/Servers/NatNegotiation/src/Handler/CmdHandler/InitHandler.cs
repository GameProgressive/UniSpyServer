using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Response;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.NatNegotiation.Handler.CmdHandler
{

    public sealed class InitHandler : CmdHandlerBase
    {
        private new InitRequest _request => (InitRequest)base._request;
        private new InitResult _result { get => (InitResult)base._result; set => base._result = value; }
        private static Dictionary<string, NatInitInfo> _initInfoPool;
        private NatMappingInfo _mappingInfo;
        private NatInitInfo _initInfo;
        private static GameTrafficRelay.Entity.Structure.Redis.RedisClient _relayRedisClient;
        private string _searchKey;
        static InitHandler()
        {
            _initInfoPool = new Dictionary<string, NatInitInfo>();
            _relayRedisClient = new GameTrafficRelay.Entity.Structure.Redis.RedisClient();
        }
        public InitHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new InitResult();
        }
        protected override void RequestCheck()
        {
            base.RequestCheck();
            _searchKey = $"{_request.Cookie} {_request.ClientIndex} {_request.Version}";
            // note: async socket may cause problem when adding _initInfo to _initInfoPool, requires a lock
            lock (_initInfoPool)
            {
                if (!_initInfoPool.Keys.Contains(_searchKey))
                {
                    _initInfo = new NatInitInfo()
                    {
                        ServerID = _client.Session.Server.ServerID,
                        Cookie = (uint)_request.Cookie,
                        UseGamePort = _request.UseGamePort,
                        ClientIndex = (NatClientIndex)_request.ClientIndex,
                        Version = _request.Version
                    };
                    _initInfoPool.TryAdd(_searchKey, _initInfo);
                }
                else
                {
                    _initInfo = _initInfoPool[_searchKey];
                }
            }
        }
        protected override void DataOperation()
        {
            _mappingInfo = new NatMappingInfo()
            {
                Version = _request.Version,
                PortType = _request.PortType,
                PublicIPEndPoint = _client.Session.RemoteIPEndPoint,
                PrivateIPEndPoint = _request.PrivateIPEndPoint
            };
            // note: async socket may cause problem when adding _mappingInfo to _initInfo.NatMappingInfos, requires a lock
            lock (_initInfo)
            {
                if (!_initInfo.NatMappingInfos.ContainsKey((NatPortType)_request.PortType))
                {
                    _initInfo.NatMappingInfos.Add((NatPortType)_request.PortType, _mappingInfo);
                }
            }

            _result.RemoteIPEndPoint = _client.Session.RemoteIPEndPoint;
        }
        protected override void ResponseConstruct()
        {
            _response = new InitResponse(_request, _result);
        }
        protected override void Response()
        {
            base.Response();

            if (_client.Info.ClientIndex is null)
            {
                _client.Info.ClientIndex = _request.ClientIndex;
            }
            if (_client.Info.Cookie is null)
            {
                _client.Info.Cookie = _request.Cookie;
            }
            // note: async socket may cause multiple call of natnegotiation, requires a lock
            lock (_initInfo)
            {
                if (_initInfo.NatMappingInfos.ContainsKey(NatPortType.NN1)
                && _initInfo.NatMappingInfos.ContainsKey(NatPortType.NN2)
                && _initInfo.NatMappingInfos.ContainsKey(NatPortType.NN3)
                && _initInfo.isNegotiating == false)
                {
                    _initInfo.isNegotiating = true;
                    DetectNat();
                    // we have use sync code to make sure the data is saved on redis
                    _redisClient.SetValue(_initInfo);
                    Task.Run(() => PrepareForConnecting());
                    // PrepareForConnecting();
                }
            }

        }
        /// <summary>
        /// Prepare to send connect response
        /// </summary>
        private void PrepareForConnecting()
        {
            LogWriter.Info($"Watting for negotiator's initInfo with cookie:{_initInfo.Cookie}");
            int waitCount = 1;
            // we only wait 8 seconds
            while (waitCount <= 4)
            {
                var initCount = _redisClient.Context.Count(k =>
                        k.Cookie == _request.Cookie
                        && k.Version == _request.Version);
                if (initCount == 2)
                {
                    LogWriter.Info("2 neigotiators found, start negotiating");
                    StartConnecting();
                    break;
                }
                else
                {
                    LogWriter.Info($"[{_client.Session.RemoteIPEndPoint}], cookie: {_initInfo.Cookie} have no negotiator found, retry count: {waitCount}");
                }
                waitCount++;
                Thread.Sleep(2000);
            }
            // if server can not find the client2 within 8 retry, then we log the error. 
            if (waitCount > 4)
            {
                LogWriter.Warning($"[{_client.Session.RemoteIPEndPoint}], cookie: {_initInfo.Cookie} have no negotiator found , we clean init information, please connect again.");
                lock (_initInfoPool)
                {
                    if (_initInfoPool.ContainsKey(_searchKey))
                    {
                        _initInfoPool.Remove(_searchKey);
                    }
                }
            }
        }

        /// <summary>
        ///  Start connect handler to tell each other's public ip and port
        /// </summary>
        private void StartConnecting()
        {
            var request = new ConnectRequest
            {
                Version = _initInfo.Version,
                Cookie = _initInfo.Cookie,
                ClientIndex = _initInfo.ClientIndex,
                IsUsingRelay = _initInfo.IsUsingRelayServer
            };
            new ConnectHandler(_client, request).Handle();
        }

        private void DetectNat()
        {
            IPEndPoint clientRemoteIPEnd;
            // if initInfo contains GP key which mean it is game server, we need to send this port to negotiator
            if (_initInfo.NatMappingInfos.ContainsKey(NatPortType.GP))
            {
                clientRemoteIPEnd = _initInfo.NatMappingInfos[NatPortType.GP].PublicIPEndPoint;
            }
            else
            {
                clientRemoteIPEnd = _initInfo.NatMappingInfos[NatPortType.NN3].PublicIPEndPoint;
            }
            var clientNatProperty = AddressCheckHandler.DetermineNatType(_initInfo);
            var guessedClientIPEndPoint = AddressCheckHandler.GuessTargetAddress(clientNatProperty, _initInfo);
            if (clientNatProperty.NatType == NatType.Symmetric || clientNatProperty.NatType == NatType.Unknown)
            {
                _initInfo.IsUsingRelayServer = true;
                var relayServers = _relayRedisClient.Context.ToList();
                var relayEndPoint = relayServers.OrderBy(x => x.ClientCount).First().PublicIPEndPoint;
                _initInfo.GuessedPublicIPEndPoint = relayEndPoint;
            }
            else
            {
                _initInfo.GuessedPublicIPEndPoint = guessedClientIPEndPoint;
                LogWriter.Debug($"client real: {clientRemoteIPEnd}, guessed: {guessedClientIPEndPoint}");
            }
        }
    }
}
