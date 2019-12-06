using System;
using System.Collections.Generic;
using System.Text;
using Chat.Structure;

namespace Chat.Handler.Command.LOGIN
{
    public class LOGINHandler
    {
        public static void Handle(ChatSession session, string[] recv)
        {
            int namespaceid = 0;
            if (Int32.TryParse(recv[1], out namespaceid))
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
        }
    }
}
