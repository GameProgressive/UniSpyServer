using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Logging;
using Serilog.Events;

namespace Chat.Abstraction.BaseClass
{
    public class ChatLogedInHandlerBase : ChatCommandHandlerBase
    {
        public ChatLogedInHandlerBase(ISession session, ChatRequestBase request) : base(session, request)
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
