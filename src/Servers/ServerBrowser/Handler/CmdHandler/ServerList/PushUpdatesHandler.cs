using UniSpyLib.Abstraction.Interface;
using QueryReport.Entity.Structure;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Structure;
using System.Linq;

namespace ServerBrowser.Handler.CmdHandler
{
    /// <summary>
    /// Search peer to peer game servers to client
    /// </summary>
    public class PushUpdatesHandler : UpdateOptionHandlerBase
    {
        public PushUpdatesHandler(IUniSpySession session, IUniSpyRequest request) : base(session,request)
        {
        }

        protected override void RequestCheck()
        {
            base.RequestCheck();
            _gameServers = GameServerInfo.RedisOperator.GetMatchedKeyValues(_request.GameName).Values
                .ToList();

            // we need to reply to client even if there are no server


            //TODO do filter
            //**************Currently we do not handle filter**********************
        }

        protected override void ResponseConstruct()
        {
            base.ResponseConstruct();
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
