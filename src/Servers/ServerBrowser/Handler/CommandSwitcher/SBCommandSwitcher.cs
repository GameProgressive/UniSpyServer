using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Logging;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Handler.CommandHandler.AdHoc.SendMessage;
using ServerBrowser.Handler.CommandHandler.AdHoc.ServerInfo;
using ServerBrowser.Handler.CommandHandler.NatNeg;
using ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionSwitcher;
using System.Linq;

namespace ServerBrowser.Handler.CommandSwitcher
{
    public class SBCommandSwitcher
    {
        public static void Switch(ISession session, byte[] recv)
        {
            if (recv.Take(6).SequenceEqual(NATNegotiation.Entity.Structure.Request.NNRequestBase.MagicData))
            {
                new NatNegCookieHandler(session, recv).Handle();
                return;
            }

            //we do not need to handle GOA query because it is handled by game server
            switch ((SBClientRequestType)recv[2])
            {
                case SBClientRequestType.ServerListRequest:
                    SBUpdateOptionSwitcher.Switch(session, recv);
                    break;
                case SBClientRequestType.ServerInfoRequest:
                    new ServerInfoHandler(session, recv).Handle();
                    break;
                case SBClientRequestType.PlayerSearchRequest:
                    break;
                case SBClientRequestType.MapLoopRequest:
                    break;
                case SBClientRequestType.SendMessageRequest:
                    //TODO
                    //Cryptorx's game use this command
                    new SendMessageHandler(session, recv).Handle();
                    break;
                default:
                    LogWriter.UnknownDataRecieved(recv);
                    break;
            }
        }
    }
}
