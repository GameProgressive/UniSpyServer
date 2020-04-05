using System.Collections.Generic;
using System.Text;
using GameSpyLib.Encryption;
using GameSpyLib.Extensions;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure;
using ServerBrowser.Entity.Structure.Packet.Request;

namespace ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.PushUpdates
{
    /// <summary>
    /// Search peer to peer game servers
    /// </summary>
    public class PushUpdatesHandler : UpdateOptionHandlerBase
    {
        public PushUpdatesHandler(ServerListRequest request) : base(request)
        {
        }

        public override void ConstructResponse(SBSession session, byte[] recv)
        {
            base.ConstructResponse(session, recv);
            //add server key number
            _dataList.Add(0);
            //add unique values number
            _dataList.Add(0);
            //add end server flag
            _dataList.AddRange(SBStringFlag.AllServerEndFlag);
        }
    }
}
