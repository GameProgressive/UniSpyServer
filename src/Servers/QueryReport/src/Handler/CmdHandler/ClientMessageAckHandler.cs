using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;

using UniSpyServer.Servers.QueryReport.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.QueryReport.Handler.CmdHandler
{
    
    public sealed class ClientMessageAckHandler : CmdHandlerBase
    {
        public ClientMessageAckHandler(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void DataOperation()
        {
            base.DataOperation();
            LogWriter.Info("Get client message ack.");
        }
    }
}