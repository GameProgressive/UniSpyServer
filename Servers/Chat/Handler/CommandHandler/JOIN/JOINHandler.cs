using System;
namespace Chat.Handler.CommandHandler.JOIN
{
    public class JOINHandler
    {
        public static void Handler(ChatSession session, string[] recv)
        {
            session.SendAsync("spyguy!spyguy@peerchat.gamespy.com \r\n JOIN #GPG!-1062731775");
        }
    }
}
