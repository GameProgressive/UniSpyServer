using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.SystemHandler.ChatSessionManage;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class QUITHandler : ChatLogedInHandlerBase
    {
        new QUIT _request;
        public QUITHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = new QUIT(request.RawRequest); 
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
