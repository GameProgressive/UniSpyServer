using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Response.General;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CommandHandler.ChatGeneralCommandHandler
{
    public class PINGHandler : ChatCommandHandlerBase
    {
        new PINGRequest _request;
        public PINGHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (PINGRequest)request;
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            _sendingBuffer = PINGReply.BuildPingReply(_session.UserInfo);
        }
    }
}
