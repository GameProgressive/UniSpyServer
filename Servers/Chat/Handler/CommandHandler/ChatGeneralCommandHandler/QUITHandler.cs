using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.SystemHandler.ChatSessionManage;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatGeneralCommandHandler
{
    public class QUITHandler : ChatLogedInHandlerBase
    {
        new QUITRequest _request;
        public QUITHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = new QUITRequest(request.RawRequest); 
        }
        protected override void CheckRequest()
        {
            base.CheckRequest();
            if (!_request.Parse())
            {
                _errorCode = ChatError.Parse;
                return;
            }
        }
        protected override void DataOperation()
        {
            base.DataOperation();

            foreach (var channel in _session.UserInfo.JoinedChannels)
            {
                channel.LeaveChannel(_session,_request.Reason);
            }
            ChatSessionManager.RemoveSession(_session);
        }
    }
}
