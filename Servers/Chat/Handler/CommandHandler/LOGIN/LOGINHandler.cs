using Chat.Entity.Structure;
using System;

namespace Chat.Handler.CommandHandler.LOGIN
{
    public class LOGINHandler
    {
        public static void Handle(ChatSession session, string[] recv)
        {
            int namespaceid = 0;
            if (!Int32.TryParse(recv[1], out namespaceid))
            {
                session.SendCommand(ChatError.MoreParameters, "LOGIN :Not enough parameters");
                session.Disconnect();
                return;
            }

            session.chatUserInfo.namespaceID = namespaceid;

            if (recv[2] == "*")
            {
                // Profile login, not handled yet!
                session.SendCommand(ChatError.MoreParameters, "LOGIN :Not handled yet!");
                session.Disconnect();
                return;
            }

            // Unique nickname login
            session.chatUserInfo.uniqueNickname = recv[3];
            //session.chatUserInfo.password = recv[4];

            session.SendCommand(ChatRPL.Login, "* 1 1");

            NICK.NICKHandler.Handle(session, recv);

            session.Send("JOIN #retrospy\r\n");
            session.SendCommand(ChatRPL.ToPic, "#retrospy Test!");
            session.SendCommand(ChatRPL.EndOfNames, "#retrospy :End of names LIST");

        }
    }
}
