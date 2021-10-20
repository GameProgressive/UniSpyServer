using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Chat.Abstraction.BaseClass
{
    public abstract class LogedInHandlerBase : CmdHandlerBase
    {
        public LogedInHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
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
