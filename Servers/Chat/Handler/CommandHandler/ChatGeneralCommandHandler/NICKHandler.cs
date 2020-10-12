using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Entity.Structure.ChatResponse.ChatGeneralResponse;
using Chat.Handler.SystemHandler.ChatSessionManage;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatGeneralCommandHandler
{
    public class NICKHandler : ChatCommandHandlerBase
    {
        new NICKRequest _request;
        public NICKHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = new NICKRequest(request.RawRequest);
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();
            if (!_request.Parse())
            {
                _errorCode = ChatError.Parse;
                return;
            }

            if (ChatSessionManager.IsNickNameExisted(_request.NickName))
            {
                _errorCode = ChatError.IRCError;
                return;
            }
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            _session.UserInfo.SetNickName(_request.NickName);
        }

        protected override void BuildErrorResponse()
        {
            base.BuildErrorResponse();
            _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            _sendingBuffer = NICKReply.BuildWelcomeReply(_session.UserInfo);
        }
    }
}
