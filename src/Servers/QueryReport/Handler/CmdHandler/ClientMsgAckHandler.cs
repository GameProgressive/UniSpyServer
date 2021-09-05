using QueryReport.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace QueryReport.Handler.CmdHandler
{
    /// <summary>
    /// Client message acknowledgement handler
    /// </summary>
    internal sealed class ClientMsgAckHandler : QRCmdHandlerBase
    {
        public ClientMsgAckHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            throw new System.NotImplementedException();
        }

        protected override void ResponseConstruct()
        {
            throw new System.NotImplementedException();
        }
    }
}
