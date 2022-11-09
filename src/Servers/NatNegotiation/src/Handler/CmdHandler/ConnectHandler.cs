using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Exception;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Response;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

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
        public ConnectHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new ConnectResult();
        }
        protected override void DataOperation()
        {
            // we send client2's ip to client1
            NatClientIndex client2Index = (NatClientIndex)(1 - (int)_request.ClientIndex);
            var client2InitInfo = _redisClient.Context.Where(k =>
                k.ServerID == _client.Connection.Server.ServerID
                && k.Cookie == _client.Info.Cookie
                && k.ClientIndex == client2Index).First();

            var responseToClient1 = new ConnectResponse(
                _request,
                new ConnectResult { RemoteEndPoint = client2InitInfo.GuessedPublicIPEndPoint });


            _client.Send(responseToClient1);
        }
    }
}
