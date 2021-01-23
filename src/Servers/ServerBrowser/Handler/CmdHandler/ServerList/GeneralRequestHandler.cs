using QueryReport.Entity.Structure;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Packet.Response;
using ServerBrowser.Entity.Structure.Result;
using System.Linq;
using UniSpyLib.Abstraction.Interface;

namespace ServerBrowser.Handler.CmdHandler
{
    internal sealed class GeneralRequestHandler : ServerListHandlerBase
    {
        private new GeneralRequestResult _result
        {
            get => (GeneralRequestResult)base._result;
            set => base._result = value;
        }
        public GeneralRequestHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new GeneralRequestResult();
        }
        /// <summary>
        /// we need to send empty server list response to game,
        /// even if there are no severs online
        /// </summary>
        protected override void DataOperation()
        {
            var searchKey = GameServerInfo.RedisOperator.BuildSearchKey(_request.GameName);
            _result.GameServerInfos = GameServerInfo.RedisOperator.GetMatchedKeyValues(searchKey).Values.ToList();
            _result.Flag = GameServerFlags.HasKeysFlag;
        }

        protected override void ResponseConstruct()
        {
            _response = new GeneralRequestResponse(_request, _result);
        }
    }
}
