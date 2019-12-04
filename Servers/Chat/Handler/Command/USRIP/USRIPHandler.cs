using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Handler.Command.USRIP
{
    public class USRIPHandler
    {
        public static void Handle(ChatSession session, string[] recv)
        {
            string sessionIP = session.Remote.ToString();
            string IP = sessionIP.Substring(0, sessionIP.IndexOf(':'));

            session.SendCommand(302, session.chatUserInfo.nickname + " :" + session.chatUserInfo.nickname + "=+ " + session.chatUserInfo.username + "@" + IP);
        }
    }
}
