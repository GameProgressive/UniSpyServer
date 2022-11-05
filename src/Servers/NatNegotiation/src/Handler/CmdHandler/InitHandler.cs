using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Exception;
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
        private static ConcurrentDictionary<string, NatInitInfo> _initInfoPool;
        private NatMappingInfo _mappingInfo;
        private NatInitInfo _initInfo;
        private static GameTrafficRelay.Entity.Structure.Redis.RedisClient _relayRedisClient;

        static InitHandler()
        {
            _initInfoPool = new ConcurrentDictionary<string, NatInitInfo>();
            _relayRedisClient = new GameTrafficRelay.Entity.Structure.Redis.RedisClient();
        }
        public InitHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new InitResult();
        }
        protected override void RequestCheck()
        {
            base.RequestCheck();

            var searchKey = $"{_request.Cookie} {_request.ClientIndex}";
            if (!_initInfoPool.Keys.Contains(searchKey))
            {
                _initInfo = new NatInitInfo()
                {
                    ServerID = _client.Session.Server.ServerID,
                    Cookie = (uint)_request.Cookie,
                    UseGamePort = _request.UseGamePort,
                    ClientIndex = (NatClientIndex)_request.ClientIndex,
                    Version = _request.Version
                };
                _initInfoPool.TryAdd(searchKey, _initInfo);
            }
            else
            {
                _initInfo = _initInfoPool[searchKey];
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
            _initInfo.NatMappingInfos.AddOrUpdate((NatPortType)_request.PortType, _mappingInfo, (s, i) => i = _mappingInfo);
            //             if(!_initInfo.NatMappingInfos.Contains((NatPortType)_request.PortType))
            //             {
            // _initInfo.NatMappingInfos.Add(_request.PortType,)
            //             }
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

            if (_initInfo.NatMappingInfos.Count == 4)
            {
                DetectNat();
                // new Thread(() =>
                //                 {
                //                     Thread.CurrentThread.IsBackground = true;
                _redisClient.SetValue(_initInfo);
                // }).Start();
                // Task.Run(()=>PrepareForConnecting());
                new Thread(() =>
                                {
                                    Thread.CurrentThread.IsBackground = true;
                                    PrepareForConnecting();
                                }).Start();
            }
        }
        /// <summary>
        /// Prepare to send connect response
        /// </summary>
        private void PrepareForConnecting()
        {
            int waitCount = 0;
            // we only wait 8 seconds
            while (waitCount++ < 8)
            {
                Thread.Sleep(1000);
                var initCount = _redisClient.Context.Count(k =>
                        k.Cookie == _request.Cookie
                        && k.Version == _request.Version);
                if (initCount == 2)
                {
                    StartConnecting();
                    break;
                }
                else
                {
                    throw new NNException("No user pair found we continue waitting.");
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
            var clientRemoteIPEnd = _initInfo.NatMappingInfos[NatPortType.GP].PublicIPEndPoint;
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
