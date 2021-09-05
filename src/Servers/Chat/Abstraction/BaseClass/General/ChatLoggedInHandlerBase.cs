using Serilog.Events;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace Chat.Abstraction.BaseClass
{
    internal abstract class ChatLogedInHandlerBase : ChatCmdHandlerBase
    {
        public ChatLogedInHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        public override void Handle()
        {
            if (!_session.UserInfo.IsLoggedIn)
            {
                LogWriter.Info("Please login first!");
                return;
            }

            base.Handle();
        }
    }
}
