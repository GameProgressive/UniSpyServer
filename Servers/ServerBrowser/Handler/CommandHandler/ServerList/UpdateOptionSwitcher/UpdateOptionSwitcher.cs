using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure.Packet.Request;
using ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.GeneralRequest;
using ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.NoServerList;
using ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.PushUpdates;
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
                session.ToLog(Serilog.Events.LogEventLevel.Error, ErrorMessage.GetErrorMsg(SBErrorCode.Parse));
                return;
            }
            switch (request.UpdateOption)
            {
                case SBServerListUpdateOption.NoServerList:
                    new NoServerListHandler(request).Handle(session, recv);
                    break;
                case SBServerListUpdateOption.GeneralRequest:
                    new GeneralRequestHandler(request).Handle(session, recv);
                    break;
                case SBServerListUpdateOption.SendGroups:
                    new SendGroupsHandler(request).Handle(session, recv);
                    break;
                case SBServerListUpdateOption.LimitResultCount:
                    break;
                case SBServerListUpdateOption.PushUpdates:
                    // worms 3d send this after join group room
                    // we should send adhoc servers which are in this room to worms3d
                    //This is FULL RULES!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    new PushUpdatesHandler(request).Handle(session, recv);
                    break;
            }
        }
    }
}
