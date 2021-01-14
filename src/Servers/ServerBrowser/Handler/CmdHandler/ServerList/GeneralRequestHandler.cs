using QueryReport.Entity.Structure;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Structure;
using System.Linq;
using UniSpyLib.Abstraction.Interface;

namespace ServerBrowser.Handler.CmdHandler
{
    public class GeneralRequestHandler : UpdateOptionHandlerBase
    {
        public GeneralRequestHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        /// <summary>
        /// we need to send empty server list response to game,
        /// even if there are no severs online
        /// </summary>
        protected override void DataOperation()
        {
            base.DataOperation();
            var searchKey = GameServerInfo.RedisOperator.BuildSearchKey(_request.GameName);
            _gameServers = GameServerInfo.RedisOperator.GetMatchedKeyValues(searchKey).Values.ToList();
        }

        protected override void ResponseConstruct()
        {

            base.ResponseConstruct();

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
