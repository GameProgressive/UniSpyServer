using UniSpyLib.Abstraction.Interface;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Structure.Request;

namespace QueryReport.Handler.CommandHandler
{
    /// <summary>
    /// Client message acknowledgement handler
    /// </summary>
    public class ClientMsgAckHandler : QRCommandHandlerBase
    {
        public ClientMsgAckHandler(IUniSpySession session,IUniSpyRequest request) : base(session, request)
        {
        }
    }
}
