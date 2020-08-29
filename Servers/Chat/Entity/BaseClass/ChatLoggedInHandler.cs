using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;

namespace Chat.Handler.CommandHandler
{
    public class ChatLogedInHandlerBase : ChatCommandHandlerBase
    {
        public ChatLogedInHandlerBase(ISession session, ChatRequestBase cmd) : base(session, cmd)
        {
        }

        public override void Handle()
        {
            if (!_session.UserInfo.IsLoggedIn)
            {
                LogWriter.ToLog(Serilog.Events.LogEventLevel.Error, "Please login first!");
                return;
            }

            base.Handle();
        }
    }
}
