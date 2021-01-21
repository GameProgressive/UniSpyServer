using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Response.General;
using Chat.Handler.SystemHandler.ChatSessionManage;
using System.Linq;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.General
{
    public class WHOISHandler : ChatCmdHandlerBase
    {
        protected new WHOISRequest _request { get { return (WHOISRequest)base._request; } }
        private ChatUserInfo _userInfo;
        public WHOISHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            base.RequestCheck();

            var result = from s in ChatSessionManager.Sessions.Values
                         where s.UserInfo.NickName == _request.NickName
                         select s.UserInfo;

            if (result.Count() != 1)
            {
                _errorCode = ChatErrorCode.NoSuchNick;
                return;
            }
            _userInfo = result.FirstOrDefault();
        }

        protected override void BuildErrorResponse()
        {
            base.BuildErrorResponse();
            _sendingBuffer = ChatIRCErrorCode.BuildNoSuchNickError();
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            _sendingBuffer = WHOISReply.BuildWhoIsUserReply(_userInfo);
        }
    }
}
