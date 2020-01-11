using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Handler.CommandHandler.USER
{
    class USERHandler
    {
        public static void Handle(ChatSession session, string[] recv)
        {
            int encryptIPSeparator = recv[1].IndexOf('|');

            if (encryptIPSeparator == -1)
                session.chatUserInfo.username = recv[1];
            else
            {
                // <crypted IP>|<unique nickname ID>
                string cryptedIP = recv[1].Substring(0, encryptIPSeparator);
                string uniqueNickID = recv[1].Substring(encryptIPSeparator, recv[1].Length - encryptIPSeparator);
                Console.WriteLine("Temp {0} {1}", cryptedIP, uniqueNickID);
            }

            session.chatUserInfo.serverIP = recv[3];
        }
    }
}
