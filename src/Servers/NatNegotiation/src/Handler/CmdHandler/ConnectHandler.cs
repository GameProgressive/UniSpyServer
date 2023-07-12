using System.Linq;
using System.Net;
using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Application;
using UniSpy.Server.NatNegotiation.Enumerate;
using UniSpy.Server.NatNegotiation.Aggregate.Redis;
using UniSpy.Server.NatNegotiation.Contract.Request;
using UniSpy.Server.NatNegotiation.Contract.Response;
using UniSpy.Server.NatNegotiation.Contract.Result;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.NatNegotiation.Aggregate.GameTrafficRelay;

namespace UniSpy.Server.NatNegotiation.Handler.CmdHandler
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
        public ConnectHandler(Client client, ConnectRequest request) : base(client, request)
        {
            _result = new ConnectResult();
        }
        protected override void RequestCheck()
        {
            // detecting nat
            var addressInfos = StorageOperation.Persistance.GetInitInfos(_client.Server.Id, (uint)_client.Info.Cookie);
            // if (addressInfos.Count < InitHandler.InitPacketCount)
            // {
            //     throw new NatNegotiation.Exception($"The number of init info in redis with cookie: {_client.Info.Cookie} is not bigger than 7.");
            // }
            var otherClientIndex = (NatClientIndex)(1 - (int)_request.ClientIndex);
            // we need both info to determine nat type
            _othersInitInfo = new NatInitInfo(addressInfos.Where(i => i.ClientIndex == otherClientIndex).OrderBy(i => i.PortType).ToList());
            _myInitInfo = new NatInitInfo(addressInfos.Where(i => i.ClientIndex == _request.ClientIndex).OrderBy(i => i.PortType).ToList());
        }

        // NOTE: If GTR is not used and both clients are on the same LAN, we need to use PrivateIPEndPoint.
        protected override void DataOperation()
        {
            var info = new NatNegotiation.Aggregate.Redis.Fail.NatFailInfo(_myInitInfo, _othersInitInfo);
            NatStrategyType strategy;

            if (StorageOperation.Persistance.GetNatFailInfo(info) == 0)
            {
                strategy = DetermineNATStrategy(_myInitInfo, _othersInitInfo);
            }
            else
            {
                _client.LogInfo("The public ip is in nat fail record database, we use game traffic relay.");
                strategy = NatStrategyType.UseGameTrafficRelay;
            }
            switch (strategy)
            {
                case NatStrategyType.UsePublicIP:
                    UsingPublicAddressToNegotiate();
                    break;
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
                case NatStrategyType.UsePrivateIP:
                    // temprary use GTR
                    UsingPrivateAddressToNegotiate();
                    break;
                case NatStrategyType.UseGameTrafficRelay:
                    UsingGameRelayServerToNegotiate();
                    break;
            }
            _client.LogInfo($"My NAT type: {_myInitInfo.NatType}, others NAT type: {_othersInitInfo.NatType} Strategy: {strategy}, Guessing IPEndPoint: {_guessedOthersIPEndPoint}");
        }

        protected override void ResponseConstruct()
        {
            _response = new ConnectResponse(
                _request,
                new ConnectResult { RemoteEndPoint = _guessedOthersIPEndPoint });
        }

        private void UsingPublicAddressToNegotiate()
        {
            _guessedOthersIPEndPoint = _othersInitInfo.PublicIPEndPoint;
        }
        private void UsingPrivateAddressToNegotiate()
        {
            _guessedOthersIPEndPoint = _othersInitInfo.PrivateIPEndPoint;
        }
        private void UsingGameRelayServerToNegotiate()
        {
            var relayServers = StorageOperation.Persistance.GetAvaliableRelayServers();
            if (relayServers.Count == 0)
            {
                throw new NatNegotiation.Exception("No GameRelayServer found, you must start a GameRelayServer!");
            }
            //todo the optimized server will be selected
            var relayEndPoint = relayServers.OrderBy(x => x.ClientCount).First().PublicIPEndPoint;
            var myIPs = _myInitInfo.AddressInfos.Select(x=>x.Value.PublicIPEndPoint.ToString()).ToList();
            var otherIPs = _othersInitInfo.AddressInfos.Select(x=>x.Value.PublicIPEndPoint.ToString()).ToList();
            var req = new NatNegotiationRequest()
            {
                Cookie = _myInitInfo.Cookie,
                ServerId = _client.Server.Id,
                GameClientIPs = _myInitInfo.ClientIndex == NatClientIndex.GameClient ? myIPs : otherIPs,
                GameServerIPs = _myInitInfo.ClientIndex == NatClientIndex.GameServer ? myIPs : otherIPs
            };
            var client = new RestClient($"http://{relayEndPoint}/NatNegotiation").UseNewtonsoftJson();
            var request = new RestRequest().AddJsonBody(req);
            var resp = client.Post<NatNegotiationResponse>(request);
            if (resp.Port == -1)
            {
                throw new NatNegotiation.Exception(resp.Message);
            }
            // we create endpoint by using the relay server address and the relay port
            _guessedOthersIPEndPoint = new IPEndPoint(relayEndPoint.Address, resp.Port);
        }

        public static NatStrategyType DetermineNATStrategy(NatInitInfo info1, NatInitInfo info2)
        {
            bool IsBothHaveSamePublicIP = info1.PublicIPEndPoint.Address.Equals(info2.PublicIPEndPoint.Address);
            // whether first 3 bytes of ip address is same, like 192.168.1.101 and 192.168.1.102
            bool IsBothInSamePrivateIPRange = info1.PrivateIPEndPoint.Address.GetAddressBytes().Take(3)
                                                .SequenceEqual(info2.PrivateIPEndPoint.Address.GetAddressBytes().Take(3));
            if (info1.NatType < NatType.Symmetric && info2.NatType < NatType.Symmetric)
            {
                if (IsBothHaveSamePublicIP)
                {
                    if (IsBothInSamePrivateIPRange)
                    {
                        return NatStrategyType.UsePrivateIP;

                    }
                    else
                    {
                        return NatStrategyType.UseGameTrafficRelay;
                    }
                }
                else
                {
                    return NatStrategyType.UsePublicIP;
                }
            }
            else
            {
                return NatStrategyType.UseGameTrafficRelay;
            }
        }
    }
}
