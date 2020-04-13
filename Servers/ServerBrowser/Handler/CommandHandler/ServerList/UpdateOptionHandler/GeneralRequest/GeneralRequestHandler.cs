using System.Collections.Generic;
using QueryReport.Entity.Structure;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure;
using ServerBrowser.Entity.Structure.Packet.Request;

namespace ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.GeneralRequest
{
    public class GeneralRequestHandler : UpdateOptionHandlerBase
    {
        public GeneralRequestHandler(ServerListRequest request) : base(request)
        {
        }

        protected override void DataOperation(SBSession session, byte[] recv)
        {
            base.DataOperation(session, recv);
            _gameServers =
                        GameServer.GetServers(_request.GameName);

            if (_gameServers == null || _gameServers.Count == 0)
            {
                _errorCode = SBErrorCode.NoServersFound;
                return;
            }
        }

        protected override void ConstructResponse(SBSession session, byte[] recv)
        {

            base.ConstructResponse(session, recv);

            GenerateServerKeys();
            //we use NTS string so total unique value list is 0
            GenerateUniqueValue();
            //add server infomation such as public ip etc.
            GenerateServersInfo();
            //after all server information is added we add the end flag
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
