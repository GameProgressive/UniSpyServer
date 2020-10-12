using System.Linq;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Entity.Structure.ChatResponse.ChatGeneralResponse;
using Chat.Entity.Structure.ChatUser;
using Chat.Handler.SystemHandler.ChatSessionManage;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatGeneralCommandHandler
{
    public class WHOISHandler : ChatCommandHandlerBase
    {
        new WHOISRequest _request;
        ChatUserInfo _userInfo;
        public WHOISHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = new WHOISRequest(request.RawRequest);
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();

            if (!_request.Parse())
            {
                _errorCode = ChatError.Parse;
                return;
            }

            var result = from s in ChatSessionManager.Sessions.Values
                         where s.UserInfo.NickName == _request.NickName
                         select s.UserInfo;

            if (result.Count() != 1)
            {
                _errorCode = ChatError.NoSuchNick;
                return;
            }
            _userInfo = result.FirstOrDefault();
        }

        protected override void BuildErrorResponse()
        {
            base.BuildErrorResponse();
            _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            _sendingBuffer = WHOISReply.BuildWhoIsUserReply(_userInfo);
        }
    }
}
