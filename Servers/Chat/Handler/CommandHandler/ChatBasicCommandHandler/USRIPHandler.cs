using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Entity.Structure.Enumerator.Request;
using GameSpyLib.Common.Entity.Interface;
using System.Net;

namespace Chat.Handler.CommandHandler
{
    public class USRIPHandler : ChatCommandHandlerBase
    {
        new USRIP _cmd;
        public USRIPHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (USRIP)cmd;
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();

            string ip = ((IPEndPoint)_session.Socket.RemoteEndPoint).Address.ToString();

            _sendingBuffer = ChatCommandBase.BuildReply(ChatReply.UserIP, "", $"@{ip}");
        }
    }
}
