using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Handler.Command.NICK
{
    public class NICKHandler
    {
        public static string Nick = "";

        public static void Handle(ChatSession session, string[] recv)
        {
            Nick = recv[1];
            string sendingBuffer = ":" + USER.USERHandler.IP + " 001 " + Nick + " :Welcome!\r\n";
            session.SendAsync(sendingBuffer);

        }
    }
}
