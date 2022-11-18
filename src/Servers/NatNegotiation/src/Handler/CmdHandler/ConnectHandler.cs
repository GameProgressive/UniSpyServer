using System;
using System.Linq;
using System.Net;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Exception;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Misc;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Response;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.NatNegotiation.Handler.CmdHandler
{

    public sealed class ConnectHandler : CmdHandlerBase
    {
        /// <summary>
        /// Indicate the init is already finished
        /// |cooie|isFinished|
        /// </summary>
        // public static Dictionary<uint, bool> ConnectStatus;
        private new ConnectRequest _request => (ConnectRequest)base._request;
        private new ConnectResult _result { get => (ConnectResult)base._result; set => base._result = value; }
        private NatInitInfo _othersInitInfo;
        private NatInitInfo _myInitInfo;
        private IPEndPoint _guessedOthersIPEndPoint;
        private NatPunchStrategy _punchStrategy;
        /// <summary>
        /// Game relay server information storage server.
        /// </summary>
        private static GameTrafficRelay.Entity.Structure.Redis.RedisClient _relayRedisClient;
        static ConnectHandler()
        {
            _relayRedisClient = new GameTrafficRelay.Entity.Structure.Redis.RedisClient();
        }
        public ConnectHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new ConnectResult();
        }
        protected override void RequestCheck()
        {
            // detecting nat
            var initInfos = _redisClient.Context.Where(k =>
                k.ServerID == _client.Connection.Server.ServerID
                && k.Cookie == _client.Info.Cookie).ToList();
            if (initInfos.Count != 2)
            {
                throw new NNException($"The number of init info in redis with cookie: {_client.Info.Cookie} is not equal to two.");
            }
            NatClientIndex otherClientIndex = (NatClientIndex)(1 - (int)_request.ClientIndex);
            // we need both info to determine nat type
            _othersInitInfo = initInfos.Where(i => i.ClientIndex == otherClientIndex).First();
            _myInitInfo = initInfos.Where(i => i.ClientIndex == _client.Info.ClientIndex).First();
        }
        protected override void DataOperation()
        {
            var searchKey = NatReportInfo.CreateKey(_myInitInfo.PublicIPEndPoint.Address,
                                                    _myInitInfo.PrivateIPEndPoint.Address,
                                                    (Guid)_othersInitInfo.ServerID,
                                                    _othersInitInfo.PrivateIPEndPoint.Address,
                                                    (NatClientIndex)_myInitInfo.ClientIndex);

            lock (ReportHandler.NatFailRecordInfos)
            {
                if (ReportHandler.NatFailRecordInfos.ContainsKey(searchKey))
                {
                    _punchStrategy = ReportHandler.NatFailRecordInfos[searchKey];
                }
                else
                {
                    var isInSameLan = AddressCheckHandler.IsInSameLan(_myInitInfo, _othersInitInfo);
                    if (isInSameLan)
                    {
                        _punchStrategy = NatPunchStrategy.UsingPrivateIPEndpoint;
                        ReportHandler.NatFailRecordInfos[searchKey] = _punchStrategy;
                    }
                    else
                    {
                        _punchStrategy = NatPunchStrategy.UsingPublicIPEndPoint;
                        ReportHandler.NatFailRecordInfos[searchKey] = _punchStrategy;
                    }
                }
            }
            switch (_punchStrategy)
            {
                case NatPunchStrategy.UsingPublicIPEndPoint:
                    UsingPublicAddressToNatNegotiate();
                    break;
                case NatPunchStrategy.UsingPrivateIPEndpoint:
                    UsingLANAddressToNatNegotiate();
                    break;
                case NatPunchStrategy.UsingGameRelay:
                    UsingGameRelayServerToNatNegotiate();
                    break;
            }
        }
        protected override void ResponseConstruct()
        {
            _response = new ConnectResponse(
                _request,
                new ConnectResult { RemoteEndPoint = _guessedOthersIPEndPoint });
        }

        private void UsingLANAddressToNatNegotiate()
        {
            _guessedOthersIPEndPoint = _othersInitInfo.PrivateIPEndPoint;
        }
        private void UsingPublicAddressToNatNegotiate()
        {
            IPEndPoint clientRemoteIPEnd;
            // if initInfo contains GP key which mean it is game server, we need to send this port to negotiator
            if (_othersInitInfo.AddressInfos.ContainsKey(NatPortType.GP))
            {
                clientRemoteIPEnd = _othersInitInfo.AddressInfos[NatPortType.GP].PublicIPEndPoint;
            }
            else
            {
                clientRemoteIPEnd = _othersInitInfo.AddressInfos[NatPortType.NN3].PublicIPEndPoint;
            }
            var clientNatProperty = AddressCheckHandler.DetermineNatType(_othersInitInfo);
            var guessedClientIPEndPoint = AddressCheckHandler.GuessTargetAddress(clientNatProperty, _othersInitInfo);
            if (clientNatProperty.NatType == NatType.Symmetric || clientNatProperty.NatType == NatType.Unknown)
            {
                UsingGameRelayServerToNatNegotiate();
            }
            else
            {
                _guessedOthersIPEndPoint = guessedClientIPEndPoint;
                _client.LogInfo($"client real: {clientRemoteIPEnd}, guessed: {guessedClientIPEndPoint}");
            }
        }
        private void UsingGameRelayServerToNatNegotiate()
        {
            _myInitInfo.IsUsingRelayServer = true;
            var relayServers = _relayRedisClient.Context.ToList();
            var relayEndPoint = relayServers.OrderBy(x => x.ClientCount).First().PublicIPEndPoint;
            _guessedOthersIPEndPoint = relayEndPoint;
        }
    }
}
