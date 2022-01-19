using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis;
using UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass;
using UniSpyServer.Servers.ServerBrowser.Entity.Exception;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Request;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Response;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Result;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.ServerBrowser.Handler.CmdHandler
{
    /// <summary>
    /// Get full rules for a server (for example, to get
    /// player information from a server that only has basic information so far)
    /// </summary>
    public sealed class ServerInfoHandler : CmdHandlerBase
    {
        private new AdHocRequest _request => (AdHocRequest)base._request;
        private new ServerInfoResult _result { get => (ServerInfoResult)base._result; set => base._result = value; }

        public ServerInfoHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new ServerInfoResult();
        }

        protected override void DataOperation()
        {
            _result.GameServerInfo = _gameServerRedisClient.Values.Where(x => x.RemoteIPEndPoint == _request.TargetIPEndPoint).FirstOrDefault();
            //_result.Flags = hasFullrule
            //TODO if there are no server found, we still send response back to client
            if (_result.GameServerInfo == null)
            {
                throw new SBException("No server found in database.");
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new ServerInfoResponse(_request, _result);
        }
    }
}
