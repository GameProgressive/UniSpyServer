using QueryReport.Entity.Structure;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Structure.Misc;
using ServerBrowser.Entity.Structure.Packet.Response;
using ServerBrowser.Entity.Structure.Result;
using System.Linq;
using UniSpyLib.Abstraction.Interface;

namespace ServerBrowser.Handler.CmdHandler
{
    internal sealed class ServerListGeneralRequestHandler : UpdateOptionHandlerBase
    {
        public ServerListGeneralRequestHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new ServerListResult();
        }
        /// <summary>
        /// we need to send empty server list response to game,
        /// even if there are no severs online
        /// </summary>
        protected override void DataOperation()
        {
            var searchKey = GameServerInfo.RedisOperator.BuildSearchKey(_request.GameName);
            _gameServers = GameServerInfo.RedisOperator.GetMatchedKeyValues(searchKey).Values.ToList();
        }

        protected override void ResponseConstruct()
        {
            _response = new ServerListResponse(_request, _result);
        }
    }
}
