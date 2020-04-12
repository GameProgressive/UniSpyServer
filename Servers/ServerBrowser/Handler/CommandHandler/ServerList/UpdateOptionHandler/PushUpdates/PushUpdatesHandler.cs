using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSpyLib.Encryption;
using GameSpyLib.Extensions;
using QueryReport.Entity.Structure;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure;
using ServerBrowser.Entity.Structure.Packet.Request;

namespace ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.PushUpdates
{
    /// <summary>
    /// Search peer to peer game servers to client
    /// </summary>
    public class PushUpdatesHandler : UpdateOptionHandlerBase
    {
        private List<GameServer> _gameSevers;

        public PushUpdatesHandler(ServerListRequest request) : base(request)
        {
        }

        public override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);
            _gameSevers = GameServer.GetGameServers(_request.GameName);
            if (_gameSevers == null)
            {
                _errorCode = SBErrorCode.DataOperation;
                return;
            }
            string[] strPart = _request.Filter.Split('=', System.StringSplitOptions.RemoveEmptyEntries);
            //groupid=12 or groupid null or groupid=null
            //we have to determin situations
            string groupid = strPart[0];
            //_gameSevers = _gameSevers.Where(g => g.ServerData.KeyValue["groupid"] == groupid).ToList();
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
