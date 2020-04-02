namespace Chat.Handler.CommandHandler.USRIP
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
