using ServerBrowser.Handler.CommandHandler.ServerList;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Handler.CommandHandler.PlayerSearch;
using ServerBrowser.Handler.CommandHandler.ServerInfo;
using ServerBrowser.Handler.CommandHandler.MapLoop;

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
                    case SBClientRequestType.PlayerSearchRequest:
                        new PlayerSearchHandler(session, recv);
                        break;
                    case SBClientRequestType.ServerInfoRequest:
                        new ServerInfoHandler(session, recv);
                        break;
                    case SBClientRequestType.SendMessageRequest:
                        break;
                    case SBClientRequestType.KeepAliveReply:
                        break;
                    case SBClientRequestType.MapLoopRequest:
                        new MapLoopHandler(session, recv);
                        break;
                    default:
                        session.UnKnownDataReceived(recv);
                        break;
                }
            }
        }
    }
}
