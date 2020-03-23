using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure.Packet.Request;
using ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.NoServerList;
using ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.PushUpdates;
using ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.SendGroups;
using ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.SendRequestedField;

namespace ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionSwitcher
{
    public class UpdateOprionSwitcher
    {
        public static void Switch(SBSession session, byte[] recv)
        {
            ServerListRequest request = new ServerListRequest();
            if (!request.Parse(recv))
            {
                return;
            }
            switch (request.UpdateOption)
            {
                case SBServerListUpdateOption.NoServerList:
                    new NoServerListHandler(session, recv);
                    break;
                case SBServerListUpdateOption.SendRequestedField:
                    new SendRequestFieldHandler(session, recv);
                    break;
                case SBServerListUpdateOption.SendGroups:
                    new SendGroupsHandler(session, recv);
                    break;
                case SBServerListUpdateOption.PushUpdates:
                    new PushUpdatesHandler(session, recv);
                    break;
            }
        }
    }
}
