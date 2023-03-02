using UniSpy.Server.QueryReport.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.Contract.Request;
using UniSpy.Server.QueryReport.Contract.Response;
using UniSpy.Server.Core.Abstraction.Interface;
namespace UniSpy.Server.QueryReport.Handler.CmdHandler
{
    /// <summary>
    /// AvailableCheckHandler
    /// </summary>

    public sealed class AvailableHandler : CmdHandlerBase
    {
        private new AvaliableRequest _request => (AvaliableRequest)base._request;
        public AvailableHandler(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void ResponseConstruct()
        {
            _response = new AvaliableResponse(_request);
        }
    }
}
