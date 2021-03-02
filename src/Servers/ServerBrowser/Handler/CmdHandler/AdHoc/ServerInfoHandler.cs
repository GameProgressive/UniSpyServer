using QueryReport.Entity.Structure.Redis;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Request;
using ServerBrowser.Entity.Structure.Response;
using ServerBrowser.Entity.Structure.Result;
using System.Linq;
using UniSpyLib.Abstraction.Interface;

namespace ServerBrowser.Handler.CmdHandler
{
    /// <summary>
    /// Get full rules for a server (for example, to get
    /// player information from a server that only has basic information so far)
    /// </summary>
    internal class ServerInfoHandler : SBCmdHandlerBase
    {
        protected new AdHocRequest _request => (AdHocRequest)base._request;
        protected new ServerInfoResult _result
        {
            get => (ServerInfoResult)base._result;
            set => base._result = value;
        }

        public ServerInfoHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new ServerInfoResult();
        }

        protected override void DataOperation()
        {
            //_result.Flags = hasFullrule
            var searchKey = new GameServerInfoRedisKey() { RemoteIPEndPoint = _request.TargetIPEndPoint };
            var result = GameServerInfoRedisOperator.GetMatchedKeyValues(searchKey)
                .Values.Where(s => s.ServerData.KeyValue.ContainsKey("hostport"))
                .Where(s => s.ServerData.KeyValue["hostport"] == _request.TargetServerHostPort);
            //TODO if there are no server found, we still send response back to client
            if (result.Count() != 1)
            {
                _result.ErrorCode = SBErrorCode.NoServersFound;
                return;
            }
            _result.GameServerInfo = result.FirstOrDefault();
        }

        protected override void ResponseConstruct()
        {
            _response = new ServerInfoResponse(_request, _result);
        }
    }
}
