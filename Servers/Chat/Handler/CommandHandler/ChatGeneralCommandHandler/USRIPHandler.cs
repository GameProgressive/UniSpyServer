using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;
using System.Net;

namespace Chat.Handler.CommandHandler.ChatGeneralCommandHandler
{
    public class USRIPHandler : ChatCommandHandlerBase
    {
        new USRIPRequest _request;
        public USRIPHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = new USRIPRequest(request.RawRequest);
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
        protected override void ConstructResponse()
        {
            base.ConstructResponse();

            string ip = ((IPEndPoint)_session.Socket.RemoteEndPoint).Address.ToString();

            _sendingBuffer = ChatReply.BuildUserIPReply(ip);
        }
    }
}
