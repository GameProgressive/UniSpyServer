using Serilog.Events;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace Chat.Abstraction.BaseClass
{
    internal abstract class ChatLogedInHandlerBase : ChatCmdHandlerBase
    {
        protected new ChatChannelRequestBase _request => (ChatChannelRequestBase)base._request;
        protected new ChatResultBase _result
        {
            get => (ChatResultBase)base._result;
            set => base._result = value;
        }
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
