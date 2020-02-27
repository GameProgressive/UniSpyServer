using ServerBrowser.Handler.CommandHandler;

namespace ServerBrowser.Handler.CommandSwitcher
{
    public class CommandSwitcher
    {
        public static void Switch(SBSession session, byte[] recv)
        {
            new ServerListHandler(session, recv);
        }
    }
}
