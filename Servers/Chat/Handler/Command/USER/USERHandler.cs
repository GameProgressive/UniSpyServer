using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Handler.Command.USER
{
    class USERHandler
    {
        public static void Handle(ChatSession session, string[] recv)
        {
            session.chatUserInfo.serverIP = recv[3];
        }
    }
}
