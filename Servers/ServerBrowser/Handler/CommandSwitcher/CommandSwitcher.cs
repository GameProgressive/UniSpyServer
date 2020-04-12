using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Handler.CommandHandler.AdHoc.ServerInfo;
using ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionSwitcher;

namespace ServerBrowser.Handler.CommandSwitcher
{
    public class CommandSwitcher
    {
        public static void Switch(SBSession session, byte[] recv)
        {
            //we do not need to handle GOA query because it is handled by game server
            switch ((SBClientRequestType)recv[2])
            {
                case SBClientRequestType.ServerListRequest:
                    UpdateOptionSwitcher.Switch(session, recv);
                    break;
                case SBClientRequestType.ServerInfoRequest:
                    new ServerInfoHandler().Handle(session,recv);
                    break;
                case SBClientRequestType.PlayerSearchRequest:
                    break;
                case SBClientRequestType.MapLoopRequest:
                    break;
                case SBClientRequestType.SendMessageRequest:
                    break;
                default:
                    session.UnKnownDataReceived(recv);
                    break;
            }
        }
    }
}
