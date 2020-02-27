using ServerBrowser.Handler.CommandHandler.ServerListHandler;
using ServerBrowser.Entity.Enumerator;

namespace ServerBrowser.Handler.CommandSwitcher
{
    public class CommandSwitcher
    {
        public static void Switch(SBSession session, byte[] recv)
        {
            switch ((SBClientRequestType)recv[2])
            {
                case SBClientRequestType.ServerListRequest:
                    new ServerListHandler(session, recv);
                    break;
                case SBClientRequestType.ServerInfoRequest:
                    break;
                case SBClientRequestType.SendMessageRequest:
                    break;
                case SBClientRequestType.MapLoopRequest:
                    break;
                case SBClientRequestType.KeepAliveReply:
                    break;
            }

        }
    }
}
