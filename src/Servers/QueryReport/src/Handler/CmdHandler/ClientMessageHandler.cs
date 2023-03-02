using UniSpy.Server.QueryReport.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.Contract.Request;
using UniSpy.Server.QueryReport.Contract.Response;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.QueryReport.Handler.CmdHandler
{

    public sealed class ClientMessageHandler : CmdHandlerBase
    {
        private new ClientMessageRequest _request => (ClientMessageRequest)base._request;
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
