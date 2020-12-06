using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using Serilog.Events;

namespace Chat.Abstraction.BaseClass
{
    public class ChatLogedInHandlerBase : ChatCommandHandlerBase
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
