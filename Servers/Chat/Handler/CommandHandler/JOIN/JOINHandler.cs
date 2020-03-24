using System;
namespace Chat.Handler.CommandHandler.JOIN
{
    public class JOINHandler
    {
        public static void Handler(ChatSession session, string[] recv)
        {
            session.SendAsync(":<32> JOIN :<gmtest>");
        }
    }
}
