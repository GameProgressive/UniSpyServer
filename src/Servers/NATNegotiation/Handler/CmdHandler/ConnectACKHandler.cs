using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Structure;
using UniSpyLib.Logging;

namespace NATNegotiation.Handler.CmdHandler
{
    public class ConnectACKHandler : NNCommandHandlerBase
    {
        public ConnectACKHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void RequestCheck()
        {
            base.RequestCheck();
            LogWriter.ToLog("client and server successfully connected!");
        }
    }
}
