using UniSpy.Server.QueryReport.V2.Abstraction.BaseClass;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.QueryReport.Application;
using UniSpy.Server.QueryReport.V2.Contract.Request;

namespace UniSpy.Server.QueryReport.V2.Handler.CmdHandler
{

    public sealed class ClientMessageAckHandler : CmdHandlerBase
    {
        public ClientMessageAckHandler(Client client, ClientMessageAckRequest request) : base(client, request)
        {
        }
        protected override void DataOperation()
        {
            base.DataOperation();
            _client.LogInfo("Get client message ack.");
        }
    }
}