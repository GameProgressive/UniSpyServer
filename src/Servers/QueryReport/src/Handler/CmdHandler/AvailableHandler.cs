using UniSpyServer.Servers.QueryReport.V2.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Request;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Response;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
namespace UniSpyServer.Servers.QueryReport.V2.Handler.CmdHandler
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
