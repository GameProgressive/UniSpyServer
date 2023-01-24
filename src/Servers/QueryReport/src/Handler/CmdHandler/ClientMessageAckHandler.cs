using UniSpyServer.Servers.QueryReport.V2.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.V2.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.QueryReport.V2.Handler.CmdHandler
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