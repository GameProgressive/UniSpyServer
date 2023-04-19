using UniSpy.Server.QueryReport.V2.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V2.Contract.Request;
using UniSpy.Server.QueryReport.V2.Contract.Response;
using UniSpy.Server.QueryReport.Application;

namespace UniSpy.Server.QueryReport.V2.Handler.CmdHandler
{
    /// <summary>
    /// AvailableCheckHandler
    /// </summary>

    public sealed class AvailableHandler : CmdHandlerBase
    {
        private new AvaliableRequest _request => (AvaliableRequest)base._request;
        public AvailableHandler(Client client, AvaliableRequest request) : base(client, request)
        {
        }
        protected override void ResponseConstruct()
        {
            _response = new AvaliableResponse(_request);
        }
    }
}
