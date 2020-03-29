using GameSpyLib.Logging;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure.Packet.Request;
using ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.GeneralRequest;
using ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.NoServerList;
using ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.SendGroups;
using ServerBrowser.Handler.SystemHandler.Error;

namespace ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionSwitcher
{
    public class UpdateOptionSwitcher
    {
        public static void Switch(SBSession session, byte[] recv)
        {
            ServerListRequest request = new ServerListRequest();
            if (!request.Parse(recv))
            {
                session.ToLog(LogLevel.Error, ErrorMessage.GetErrorMsg(SBErrorCode.Parse));
                return;
            }
            switch (request.UpdateOption)
            {
                case SBServerListUpdateOption.NoServerList:
                    new NoServerListHandler(session, recv);
                    break;
                case SBServerListUpdateOption.GeneralRequest:
                    new GeneralRequestHandler(session, recv);
                    break;
                case SBServerListUpdateOption.SendGroups:
                    new SendGroupsHandler(session, recv);
                    break;
                case SBServerListUpdateOption.LimitResultCount:
                    break;
            }
        }
    }
}
