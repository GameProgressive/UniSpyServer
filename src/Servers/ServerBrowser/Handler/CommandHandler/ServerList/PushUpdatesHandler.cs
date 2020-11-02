using UniSpyLib.Abstraction.Interface;
using QueryReport.Entity.Structure;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure;
using ServerBrowser.Entity.Structure.Packet.Request;
using System.Linq;

namespace ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.PushUpdates
{
    /// <summary>
    /// Search peer to peer game servers to client
    /// </summary>
    public class PushUpdatesHandler : UpdateOptionHandlerBase
    {
        public PushUpdatesHandler(ServerListRequest request, ISession client, byte[] recv) : base(request, client, recv)
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
