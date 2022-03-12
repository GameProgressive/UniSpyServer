using System.Linq;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.NatNegotiation.Handler.CmdHandler
{
    /// <summary>
    /// The connect ack handler indicate that the client is already received the connect packet and start nat negotiation
    /// </summary>
    [HandlerContract(RequestType.ConnectAck)]
    public class ConnectAckHandler : CmdHandlerBase
    {
        private new ConnectAckRequest _request => (ConnectAckRequest)base._request;
        public ConnectAckHandler(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void DataOperation()
        {
            var userInfo = _redisClient.Values.FirstOrDefault(
                k => k.ServerID == _client.Session.Server.ServerID
                & k.PublicIPEndPoint == _client.Session.RemoteIPEndPoint
                & k.PortType == _request.PortType
                & k.Cookie == _request.Cookie);
            if (userInfo is null)
            {
                // we do nothing here
                return;
            }
            // currently we do not know what this for, we just keep this
            userInfo.IsGotConnectPacket = true;
            _redisClient.SetValue(userInfo);
        }
    }
}