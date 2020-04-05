using System.Net;
using Chat.Entity.Structure;

namespace Chat.Handler.CommandHandler.USRIP
{
    public class USRIPHandler : CommandHandlerBase
    {
        public USRIPHandler() : base()
        {
        }

        public override void CheckRequest(ChatSession session, string[] recv)
        {
            base.CheckRequest(session, recv);
        }

        public override void DataOperation(ChatSession session, string[] recv)
        {
            base.DataOperation(session, recv);
        }

        public override void ConstructResponse(ChatSession session, string[] recv)
        {
            base.ConstructResponse(session, recv);
            string IP = "0.0.0.0";//((IPEndPoint)session.Socket.RemoteEndPoint).Address.ToString();

           _sendingBuffer =  ChatServer.GenerateChatCommand(ChatRPL.USRIP,
                session.UserInfo.NickName
                + " :" + session.UserInfo.NickName
                + "=+ " + session.UserInfo.UserName
                + "@" +
                IP);
        }
    }
}
