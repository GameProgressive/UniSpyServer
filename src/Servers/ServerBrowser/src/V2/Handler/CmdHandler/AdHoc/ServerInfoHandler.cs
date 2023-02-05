using UniSpyServer.Servers.ServerBrowser.Application;
using UniSpyServer.Servers.ServerBrowser.V2.Abstraction.BaseClass;
using UniSpyServer.Servers.ServerBrowser.V2.Entity.Structure.Request;
using UniSpyServer.Servers.ServerBrowser.V2.Entity.Structure.Response;
using UniSpyServer.Servers.ServerBrowser.V2.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.ServerBrowser.V2.Handler.CmdHandler
{
    /// <summary>
    /// Get full rules for a server (for example, to get
    /// player information from a server that only has basic information so far)
    /// </summary>

    public sealed class ServerInfoHandler : CmdHandlerBase
    {
        private new ServerInfoRequest _request => (ServerInfoRequest)base._request;
        private new ServerInfoResult _result { get => (ServerInfoResult)base._result; set => base._result = value; }

        public ServerInfoHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new ServerInfoResult();
        }

        protected override void DataOperation()
        {
            _result.GameServerInfo = StorageOperation.Persistance.GetGameServerInfo(_request.GameServerPublicIPEndPoint);

            //TODO if there are no server found, we still send response back to client
            if (_result.GameServerInfo is null)
            {
                // throw new SBException("No server found in database.");
                _client.LogInfo($"No server found on IP {_request.GameServerPublicIPEndPoint}.");
                return;
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new ServerInfoResponse(_request, _result);
        }
    }
}