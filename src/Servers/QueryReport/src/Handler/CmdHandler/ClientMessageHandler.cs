using UniSpyServer.Servers.QueryReport.V2.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Redis;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Request;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Response;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.QueryReport.V2.Handler.CmdHandler
{

    public sealed class ClientMessageHandler : CmdHandlerBase
    {
        private new ClientMessageRequest _request => (ClientMessageRequest)base._request;
        public static RedisChannel Channel;
        public ClientMessageHandler(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void RequestCheck()
        {
            // we do not need to execute request.Parse()
            _client.LogInfo($"Received client message with cookie: {_request.Cookie}");
        }
        protected override void ResponseConstruct()
        {
            _response = new ClientMessageResponse(_request);
        }
    }
}
