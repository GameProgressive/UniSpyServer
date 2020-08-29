using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Handler.SystemHandler.ChatSessionManage;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class NICKHandler : ChatCommandHandlerBase
    {
        new NICK _request;
        public NICKHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = new NICK(request.RawRequest);
        }

        protected override void CheckRequest()
        {

            base.CheckRequest();

            if (ChatSessionManager.IsNickNameExisted(_request.NickName))
            {
                _errorCode = ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
            }

        }

        protected override void DataOperation()
        {
            base.DataOperation();
            _session.UserInfo.SetNickName(_request.NickName);
        }
    }
}
