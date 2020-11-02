using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Response;
using Chat.Entity.Structure.Response.General;
using Chat.Entity.Structure.User;
using Chat.Handler.SystemHandler.ChatSessionManage;
using UniSpyLib.Abstraction.Interface;
using System.Linq;

namespace Chat.Handler.CommandHandler.ChatGeneralCommandHandler
{
    public class WHOISHandler : ChatCommandHandlerBase
    {
        new WHOISRequest _request;
        ChatUserInfo _userInfo;
        public WHOISHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (WHOISRequest)request;
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();

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
