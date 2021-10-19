using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.contract;
using QueryReport.Entity.Enumerate;
using QueryReport.Entity.Structure.Request;
using QueryReport.Entity.Structure.Response;
using QueryReport.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;
namespace QueryReport.Handler.CmdHandler
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
