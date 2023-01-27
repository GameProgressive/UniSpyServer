using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.NatNegotiation.Handler.CmdHandler
{
    /// <summary>
    /// The connect ack handler indicate that the client is already received the connect packet and start nat negotiation
    /// </summary>

    public class ConnectAckHandler : CmdHandlerBase
    {
        private new ConnectAckRequest _request => (ConnectAckRequest)base._request;
        public ConnectAckHandler(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void DataOperation()
        {
            _client.LogInfo($"client:{_request.ClientIndex} aknowledged connect request.");
        }
    }
}