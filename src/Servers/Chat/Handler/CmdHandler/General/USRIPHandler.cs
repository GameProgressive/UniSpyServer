using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Response.General;
using UniSpyLib.Abstraction.Interface;
using System.Net;

namespace Chat.Handler.CmdHandler.ChatGeneralCommandHandler
{
    public class USRIPHandler : ChatCmdHandlerBase
    {
        new USRIPRequest _request { get { return (USRIPRequest)base._request; } }
        public USRIPHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void CheckRequest()
        {
            base.CheckRequest();
            if (!(bool)_request.Parse())
            {
                _errorCode = ChatError.Parse;
                return;
            }
        }
        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();

            string ip = ((IPEndPoint)_session.Socket.RemoteEndPoint).Address.ToString();

            _sendingBuffer = USRIPReply.BuildUserIPReply(ip);
        }
    }
}
