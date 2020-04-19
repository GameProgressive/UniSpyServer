using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Enumerator.Request;
using GameSpyLib.Common.Entity.Interface;
using System.Net;

namespace Chat.Handler.CommandHandler
{
    public class USRIPHandler : ChatCommandHandlerBase
    {
        public USRIPHandler(IClient client, ChatCommandBase cmd) : base(client, cmd)
        {
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();

            string IP = ((IPEndPoint)_session.Socket.RemoteEndPoint).Address.ToString();

            _sendingBuffer = ChatCommandBase.BuildBasicRPL(ChatResponseType.UserIP,
                 _session.ClientInfo.NickName
                 + " :" + _session.ClientInfo.NickName
                 + "=+ " + _session.ClientInfo.UserName
                 + "@" +
                 IP);
        }
    }
}
