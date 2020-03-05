using ServerBrowser.Handler.CommandHandler.ServerList;
using ServerBrowser.Entity.Enumerator;

namespace ServerBrowser.Handler.CommandSwitcher
{
    public class CommandSwitcher
    {
        public static void Switch(SBSession session, byte[] recv)
        {
            if (recv[0] == '\\')
            {
                // old server browser command
            }
            else
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
                    case SBClientRequestType.KeepAliveReply:
                        break;
                    case SBClientRequestType.MapLoopRequest:
                        break;
                    default:
                        session.UnKnownDataReceived(recv);
                        break;
                }
            }
        }
    }
}
