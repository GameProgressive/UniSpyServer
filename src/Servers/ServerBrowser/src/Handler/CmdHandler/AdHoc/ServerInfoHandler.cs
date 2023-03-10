using UniSpy.Server.ServerBrowser.Application;
using UniSpy.Server.ServerBrowser.Abstraction.BaseClass;
using UniSpy.Server.ServerBrowser.Contract.Request;
using UniSpy.Server.ServerBrowser.Contract.Response;
using UniSpy.Server.ServerBrowser.Contract.Result;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.ServerBrowser.Handler.CmdHandler
{
    /// <summary>
    /// Get full rules for a server (for example, to get
    /// player information from a server that only has basic information so far)
    /// </summary>

    public sealed class ServerInfoHandler : CmdHandlerBase
    {
        private new ServerInfoRequest _request => (ServerInfoRequest)base._request;
        private new AdHocResult _result { get => (AdHocResult)base._result; set => base._result = value; }

        public ServerInfoHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new AdHocResult();
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
            if (_result.GameServerInfo is null)
            {
                return;
            }
            _response = new UpdateServerInfoResponse(_result);
        }
    }
}
