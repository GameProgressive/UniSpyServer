using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.Entity.contract;
using UniSpyServer.Servers.QueryReport.Entity.Enumerate;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Request;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Response;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
namespace UniSpyServer.Servers.QueryReport.Handler.CmdHandler
{
    /// <summary>
    /// AvailableCheckHandler
    /// </summary>
    [HandlerContract(RequestType.AvaliableCheck)]
    public sealed class AvailableHandler : CmdHandlerBase
    {
        private new AvaliableRequest _request => (AvaliableRequest)base._request;
        public AvailableHandler(ISession session, IRequest request) : base(session, request)
        {
        }
        protected override void ResponseConstruct()
        {
            _response = new AvaliableResponse(_request, _result);
        }
    }
}
