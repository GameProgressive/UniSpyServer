using System.Collections.Generic;
using System.Linq;
using System.Net;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Application;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Exception;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Response;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using RestSharp;
using UniSpyServer.Servers.GameTrafficRelay;
using RestSharp.Serializers.NewtonsoftJson;

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
        public ConnectHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new ConnectResult();
        }
        protected override void RequestCheck()
        {
            // detecting nat
            var initInfos = StorageOperation.Persistance.GetInitInfos(_client.Connection.Server.ServerID, (uint)_client.Info.Cookie);
            if (initInfos.Count != 2)
            {
                throw new NNException($"The number of init info in redis with cookie: {_client.Info.Cookie} is not equal to two.");
            }
            NatClientIndex otherClientIndex = (NatClientIndex)(1 - (int)_request.ClientIndex);
            // we need both info to determine nat type
            _othersInitInfo = initInfos.Where(i => i.ClientIndex == otherClientIndex).First();
            _myInitInfo = initInfos.Where(i => i.ClientIndex == _client.Info.ClientIndex).First();
        }

        // NOTE: If GTR is not used and both clients are on the same LAN, we need to use PrivateIPEndPoint.
        protected override void DataOperation()
        {
            var isUsingRelayServer = WhetherUsingRelayServer(_myInitInfo, _othersInitInfo);
            if (isUsingRelayServer)
            {
                UsingGameRelayServerToNatNegotiate();
            }
            else
            {
                UsingPublicAddressToNatNegotiate();
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new ConnectResponse(
                _request,
                new ConnectResult { RemoteEndPoint = _guessedOthersIPEndPoint });
        }

        private void UsingPublicAddressToNatNegotiate()
        {
            _guessedOthersIPEndPoint = _othersInitInfo.PublicIPEndPoint;
        }
        private void UsingGameRelayServerToNatNegotiate()
        {
            var relayServers = GameTrafficRelay.Application.StorageOperation.Persistance.GetAvaliableRelayServers();
            if (relayServers.Count == 0)
            {
                throw new NNException("No GameRelayServer found, you must start a GameRelayServer!");
            }
            //todo the optimized server will be selected
            var relayEndPoint = relayServers.OrderBy(x => x.ClientCount).First().PublicIPEndPoint;

            var dict = new Dictionary<string, object>();
            dict.Add("Cookie", _myInitInfo.Cookie);
            dict.Add("ServerId", _client.Connection.Server.ServerID);
            var client = new RestClient($"http://{relayEndPoint}/NatNegotiation").UseNewtonsoftJson();
            var request = new RestRequest().AddJsonBody(dict);
            var resp = client.Post<NatNegotiationResponse>(request);
            if (_client.Info.ClientIndex == NatClientIndex.GameClient)
            {
                _guessedOthersIPEndPoint = resp.IPEndPoint1;
            }
            else if (_client.Info.ClientIndex == NatClientIndex.GameServer)
            {
                _guessedOthersIPEndPoint = resp.IPEndPoint2;
            }
            else
            {
                throw new NNException("The client index is not applied");
            }
        }

        public static bool WhetherUsingRelayServer(NatInitInfo info1, NatInitInfo info2)
        {
            // due to multi NAT situation, we have to check if clients are in same public ip address, and both client are no NAT
            // if two clients have one public ip like 202.91.34.188, we set the negotiate address for each other such as 
            // client1 public ip 202.91.34.188:123, private ip: 192.168.1.100
            // client2 public ip 202.91.34.188:124, private ip: 192.168.1.101
            // there are situations as follows.
            // 1. clients are in multi NAT with different router
            // 2. clients are in one NAT with same router
            // there are solutions as follows.
            // 1. we can set public ip as negotiation address, but if situation 1 happen, the router will not transfer data for each clients.
            // 2. we can set private IP as negotiation address, but if situation 1 happen, two clients can not receive each information, because they are not in same router.
            // 3. we can set private ip as negotiation address, if clients are in one NAT, small possibility
            // gamespy sdk need 100% success on nat negotiation, therefore if clients have same public ip and have NAT, to make sure 100% success, we use GameTrafficRelay server.
            bool IsBothHaveSamePublicIP = info1.PublicIPEndPoint.Address.Equals(info2.PublicIPEndPoint.Address);
            if (info1.NatType < NatType.Symmetric && info1.NatType < NatType.Symmetric && !IsBothHaveSamePublicIP)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
