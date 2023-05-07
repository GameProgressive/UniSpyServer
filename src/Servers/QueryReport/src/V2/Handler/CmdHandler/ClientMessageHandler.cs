using UniSpy.Server.QueryReport.V2.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V2.Contract.Request;
using UniSpy.Server.QueryReport.V2.Contract.Response;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.QueryReport.Application;

namespace UniSpy.Server.QueryReport.V2.Handler.CmdHandler
{

    public sealed class ClientMessageHandler : CmdHandlerBase
    {
        private new ClientMessageRequest _request => (ClientMessageRequest)base._request;
        public ClientMessageHandler(Client client, ClientMessageRequest request) : base(client, request)
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
