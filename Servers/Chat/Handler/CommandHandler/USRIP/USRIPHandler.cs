using System.Net;
using Chat.Entity.Structure;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.USRIP
{
    public class USRIPHandler : CommandHandlerBase
    {
        public USRIPHandler(IClient client, string[] recv) : base(client, recv)
        {
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();
            string IP = ((IPEndPoint)_session.Socket.RemoteEndPoint).Address.ToString();

           _sendingBuffer =  ChatServer.GenerateChatCommand(ChatRPL.USRIP,
                _session.UserInfo.NickName
                + " :" + _session.UserInfo.NickName
                + "=+ " + _session.UserInfo.UserName
                + "@" +
                IP);
        }
    }
}
