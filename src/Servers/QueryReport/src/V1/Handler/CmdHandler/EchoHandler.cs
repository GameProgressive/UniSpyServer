using UniSpy.Server.QueryReport.Application;
using UniSpy.Server.QueryReport.V1.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V1.Contract.Request;
using UniSpy.Server.QueryReport.V1.Contract.Response;

namespace UniSpy.Server.QueryReport.V1.Handler.CmdHandler
{
    /// <summary>
    /// Keep alive request
    /// </summary>
    public sealed class EchoHandler : CmdHandlerBase
    {
        private new EchoRequest _request => (EchoRequest)base._request;
        public EchoHandler(Client client, EchoRequest request) : base(client, request)
        {
        }
        protected override void ResponseConstruct()
        {
            _response = new EchoResponse(_request);
        }
    }
}