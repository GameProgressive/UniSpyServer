using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;
using System.Net;

namespace Chat.Handler.CommandHandler.ChatGeneralCommandHandler
{
    public class USRIPHandler : ChatCommandHandlerBase
    {
        public USRIPHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
        }

        protected override void ConstructResponse()
        {
            base.ConstructResponse();

            string ip = ((IPEndPoint)_session.Socket.RemoteEndPoint).Address.ToString();

            _sendingBuffer = ChatReply.BuildUserIPReply(ip);
        }
    }
}
