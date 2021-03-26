using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Structure.Request;
using QueryReport.Entity.Structure.Response;
using QueryReport.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;
namespace QueryReport.Handler.CmdHandler
{
    /// <summary>
    /// AvailableCheckHandler
    /// </summary>
    internal sealed class AvailableHandler : QRCmdHandlerBase
    {
        private new AvaliableRequest _request => (AvaliableRequest)base._request;
        private new QRDefaultResult _result
        {
            get => (QRDefaultResult)base._result;
            set => base._result = value;
        }
        public AvailableHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void ResponseConstruct()
        {
            _response = new AvaliableResponse(_request, _result);
        }
    }
}
