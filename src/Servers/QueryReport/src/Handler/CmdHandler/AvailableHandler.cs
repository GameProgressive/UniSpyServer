using UniSpyServer.QueryReport.Abstraction.BaseClass;
using UniSpyServer.QueryReport.Entity.contract;
using UniSpyServer.QueryReport.Entity.Enumerate;
using UniSpyServer.QueryReport.Entity.Structure.Request;
using UniSpyServer.QueryReport.Entity.Structure.Response;
using UniSpyServer.QueryReport.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
namespace UniSpyServer.QueryReport.Handler.CmdHandler
{
    /// <summary>
    /// AvailableCheckHandler
    /// </summary>
    [HandlerContract(RequestType.AvaliableCheck)]
    public sealed class AvailableHandler : CmdHandlerBase
    {
        private new AvaliableRequest _request => (AvaliableRequest)base._request;
        public AvailableHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void ResponseConstruct()
        {
            _response = new AvaliableResponse(_request, _result);
        }
    }
}
