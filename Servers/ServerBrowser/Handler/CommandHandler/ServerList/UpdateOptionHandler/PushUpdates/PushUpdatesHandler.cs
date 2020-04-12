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
        public PushUpdatesHandler(ServerListRequest request) : base(request)
        {
        }

        public override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);
            _gameServers = GameServer.GetGameServers(_request.GameName);
            if (_gameServers == null)
            {
                _errorCode = SBErrorCode.DataOperation;
                return;
            }
            //**************Currently we do not handle filter**********************
        }

        public override void ConstructResponse(SBSession session, byte[] recv)
        {
            base.ConstructResponse(session, recv);
            //add server key number
            GenerateServerKeys();
            //add unique values number
            GenerateUniqueValue();
            //add server info
            GenerateServersInfo();
            //add end server flag
            _dataList.AddRange(SBStringFlag.AllServerEndFlag);
        }
    }
}
