using UniSpy.Server.ServerBrowser.V2.Application;
using UniSpy.Server.ServerBrowser.V2.Abstraction.BaseClass;
using UniSpy.Server.ServerBrowser.V2.Contract.Request;
using UniSpy.Server.ServerBrowser.V2.Contract.Response;
using UniSpy.Server.ServerBrowser.V2.Contract.Result;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.ServerBrowser.V2.Handler.CmdHandler
{
    /// <summary>
    /// Get full rules for a server (for example, to get
    /// player information from a server that only has basic information so far)
    /// </summary>

    public sealed class ServerInfoHandler : CmdHandlerBase
    {
        private new ServerInfoRequest _request => (ServerInfoRequest)base._request;
        private new AdHocResult _result { get => (AdHocResult)base._result; set => base._result = value; }

        public ServerInfoHandler(Client client, ServerInfoRequest request) : base(client, request)
        {
            _result = new AdHocResult();
        }

        protected override void DataOperation()
        {
            _result.GameServerInfo = QueryReport.V2.Application.StorageOperation.Persistance.GetGameServerInfo(_request.GameServerPublicIPEndPoint);

            //TODO if there are no server found, we still send response back to client
            if (_result.GameServerInfo is null)
            {
                // throw new ServerBrowser.Exception("No server found in database.");
                _client.LogInfo($"No server found on IP {_request.GameServerPublicIPEndPoint}.");
                return;
            }
        }

        protected override void ResponseConstruct()
        {
            if (_result.GameServerInfo is null)
            {
                return;
            }
            _response = new UpdateServerInfoResponse(_result);
        }
    }
}
