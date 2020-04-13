using System.Collections.Generic;
using System.Linq;
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

        protected override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);
            _gameServers = GameServer.GetServers(_request.GameName).Where(g=>g.IsPeerServer=true).ToList();
            if (_gameServers == null)
            {
                _errorCode = SBErrorCode.DataOperation;
                return;
            }

            if (_gameServers.Count() == 0)
            {
                _errorCode = SBErrorCode.NoServersFound;
                return;
            }
            //**************Currently we do not handle filter**********************
        }

        protected override void ConstructResponse(SBSession session, byte[] recv)
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

        protected override void CheckNonStandardPort(List<byte> header, GameServer server)
        {
            if (server.IsPeerServer)
            {
                return;
            }
            base.CheckNonStandardPort(header, server);
        }
    }
}
