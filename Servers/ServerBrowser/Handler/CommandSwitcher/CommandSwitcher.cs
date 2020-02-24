using ServerBrowser.Handler.CommandHandler;
using System;

namespace ServerBrowser.Handler.CommandSwitcher
{
    public class CommandSwitcher
    {
        public static void Switch(SBSession session, string recv)
        {
            string[] requests = recv.Split(new string[] { "\0\0\0\0\0" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string request in requests)
            {
                new ServerListRetriveHandler(session, request);
            }

        }
    }
}
