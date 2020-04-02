using Chat.Entity.Structure;
using System;

namespace Chat.Handler.CommandHandler.LOGIN
{
    public class LOGINHandler : CommandHandlerBase
    {
        int _namespaceID = 0;
        public LOGINHandler(ChatSession session, string[] recv) : base(session, recv)
        {
        }

        public override void CheckRequest(ChatSession session, string[] recv)
        {
            base.CheckRequest(session, recv);
            if (!Int32.TryParse(recv[1], out _namespaceID))
            {
                _errorCode = ChatError.Parse;
                return;
            }
            session.UserInfo.NameSpaceID = _namespaceID;

            if (recv[2] == "*")
            {
                // Profile login, not handled yet!
                _errorCode = ChatError.Login_Failed;
                return;
            }

            // Unique nickname login
            session.UserInfo.UniqueNickName = recv[3];
            //session.chatUserInfo.password = recv[4];
        }

        public override void DataOperation(ChatSession session, string[] recv)
        {
            base.DataOperation(session, recv);
            session.UserInfo.NickName = recv[1];
        }

        public override void ConstructResponse(ChatSession session, string[] recv)
        {
            base.ConstructResponse(session, recv);
            _sendingBuffer = ChatServer.GenerateChatCommand(ChatRPL.Login, "* 1 1");
        }

        public override void Response(ChatSession session, string[] recv)
        {
            base.Response(session, recv);
            session.SendAsync($":{session.UserInfo.ServerIP} 001 {session.UserInfo.NickName} :Welcome!\r\n");
            //session.Send("JOIN #retrospy\r\n");
            session.SendAsync(ChatServer.GenerateChatCommand(ChatRPL.ToPic, "#retrospy Test!"));
            session.SendAsync(ChatServer.GenerateChatCommand(ChatRPL.EndOfNames, "#retrospy :End of names LIST"));
        }
    }
}
