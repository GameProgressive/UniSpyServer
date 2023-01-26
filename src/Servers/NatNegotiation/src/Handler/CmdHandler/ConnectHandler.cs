using System.Collections.Generic;
using System.Net.Http;
using System;
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
        protected override void DataOperation()
        {
            if (_myInitInfo.IsUsingRelayServer || _othersInitInfo.IsUsingRelayServer)
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
            var guessedClientIPEndPoint = _othersInitInfo.PublicIPEndPoint;
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
            var client = new RestClient($"http://{relayEndPoint}/NatNegotiation");
            client.UseNewtonsoftJson();
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
    }
}
