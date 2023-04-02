using UniSpy.Server.QueryReport.V2.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.QueryReport.V2.Handler.CmdHandler
{

    public sealed class ClientMessageAckHandler : CmdHandlerBase
    {
        public ClientMessageAckHandler(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void DataOperation()
        {
            base.DataOperation();
            _client.LogInfo("Get client message ack.");
        }
    }
}