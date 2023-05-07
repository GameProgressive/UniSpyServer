using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Contract.Request;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.NatNegotiation.Application;

namespace UniSpy.Server.NatNegotiation.Handler.CmdHandler
{
    /// <summary>
    /// The connect ack handler indicate that the client is already received the connect packet and start nat negotiation
    /// </summary>

    public class ConnectAckHandler : CmdHandlerBase
    {
        private new ConnectAckRequest _request => (ConnectAckRequest)base._request;
        public ConnectAckHandler(Client client, ConnectAckRequest request) : base(client, request)
        {
        }
        protected override void DataOperation()
        {
            _client.LogInfo($"client:{_request.ClientIndex} aknowledged connect request.");
        }
    }
}