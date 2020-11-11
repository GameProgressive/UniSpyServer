using UniSpyLib.Abstraction.Interface;
using QueryReport.Entity.Structure;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Structure;
using ServerBrowser.Entity.Structure.Packet.Request;

namespace ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.GeneralRequest
{
    public class GeneralRequestHandler : UpdateOptionHandlerBase
    {
        public GeneralRequestHandler(ServerListRequest request, ISession session, byte[] recv) : base(request, session, recv)
        {
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            _gameServers =
                        GameServer.GetServers(_request.GameName);

            //we need to send empty server list response to game
            //if (_gameServers == null || _gameServers.Count == 0)
            //{
            //    _errorCode = SBErrorCode.NoServersFound;
            //    return;
            //}
        }

        protected override void ConstructResponse()
        {

            base.ConstructResponse();

            GenerateServerKeys();
            //we use NTS string so total unique value list is 0
            GenerateUniqueValue();
            //add server infomation such as public ip etc.
            GenerateServersInfo();
            //after all server information is added we add the end flag
            _dataList.AddRange(SBStringFlag.AllServerEndFlag);
        }
    }
}
