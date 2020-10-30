using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse.ChatGeneralResponse;
using GameSpyLib.Abstraction.Interface;
using System.Net;

namespace Chat.Handler.CommandHandler.ChatGeneralCommandHandler
{
    public class USRIPHandler : ChatCommandHandlerBase
    {
        new USRIPRequest _request;
        public USRIPHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (USRIPRequest)request;
        }
        protected override void CheckRequest()
        {
            base.CheckRequest();
            if (!_request.Parse())
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
