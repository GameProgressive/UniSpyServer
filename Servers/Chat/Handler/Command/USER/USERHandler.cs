using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Handler.Command.USER
{
    class USERHandler
    {
        public static string IP = "";

        public static void Handle(ChatSession session, string[] recv)
        {
            IP = recv[2];
        }
    }
}
