using System.Collections.Generic;
using System.Linq;
using GameSpyLib.Common.Entity.Interface;
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
        public PushUpdatesHandler(ServerListRequest request,IClient client,byte[] recv) : base(request,client,recv)
        {
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();
            _gameServers = GameServer.GetServers(_request.GameName);
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

        protected override void ConstructResponse()
        {
            base.ConstructResponse();
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
