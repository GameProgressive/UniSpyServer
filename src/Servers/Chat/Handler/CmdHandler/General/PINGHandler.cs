using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Response.General;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.General
{
    public class PINGHandler : ChatCmdHandlerBase
    {
        new PINGRequest _request { get { return (PINGRequest)base._request; } }
        public PINGHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            _sendingBuffer = PINGReply.BuildPingReply(_session.UserInfo);
        }
    }
}
