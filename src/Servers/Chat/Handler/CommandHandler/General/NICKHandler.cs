using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Response;
using Chat.Entity.Structure.Response.General;
using Chat.Handler.SystemHandler.ChatSessionManage;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CommandHandler.ChatGeneralCommandHandler
{
    public class NICKHandler : ChatCommandHandlerBase
    {
        new NICKRequest _request;
        public NICKHandler(IUniSpySession session, ChatRequestBase request) : base(session, request)
        {
            _request = (NICKRequest)request;
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();

            if (ChatSessionManager.IsNickNameExisted(_request.NickName))
            {
                _errorCode = ChatError.NickNameExisted;
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
