using QueryReport.Entity.Structure.Redis;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Packet.Response;
using ServerBrowser.Entity.Structure.Result;
using System.Linq;
using UniSpyLib.Abstraction.Interface;

namespace ServerBrowser.Handler.CmdHandler
{
    internal sealed class ServerMainListHandler : ServerListUpdateOptionHandlerBase
    {
        private new ServerMainListResult _result
        {
            get => (ServerMainListResult)base._result;
            set => base._result = value;
        }
        public ServerMainListHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new ServerMainListResult();
        }
        /// <summary>
        /// we need to send empty server list response to game,
        /// even if there are no severs online
        /// </summary>
        protected override void DataOperation()
        {
            var searchKey = new GameServerInfoRedisKey()
            {
                GameName = _request.GameName
            };

            _result.GameServerInfos = GameServerInfoRedisOperator.GetMatchedKeyValues(searchKey).Values.ToList();
            _result.Flag = GameServerFlags.HasKeysFlag;
        }

        protected override void ResponseConstruct()
        {
            _response = new ServerMainListResponse(_request, _result);
        }
    }
}
