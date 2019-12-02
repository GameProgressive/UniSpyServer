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
            session.chatUserInfo.nickname = recv[1];

            string sendingBuffer = ":" + session.chatUserInfo.serverIP + " 001 " + recv[1] + " :Welcome!\r\n";
            session.SendAsync(sendingBuffer);
        }
    }
}
