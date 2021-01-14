using Serilog.Events;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace Chat.Abstraction.BaseClass
{
    public abstract class ChatLogedInHandlerBase : ChatCmdHandlerBase
    {
        public ChatLogedInHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        public override void Handle()
        {
            if (!_session.UserInfo.IsLoggedIn)
            {
                LogWriter.ToLog(LogEventLevel.Error, "Please login first!");
                return;
            }

            base.Handle();
        }
    }
}
