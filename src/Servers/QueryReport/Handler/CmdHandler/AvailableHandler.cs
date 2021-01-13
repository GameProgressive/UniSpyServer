using System.Linq;
using UniSpyLib.Abstraction.Interface;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;
using QueryReport.Entity.Structure.Request;
using QueryReport.Entity.Structure.Response;
using QueryReport.Entity.Structure.Result;
namespace QueryReport.Handler.CmdHandler
{
    /// <summary>
    /// AvailableCheckHandler
    /// </summary>
    internal sealed class AvailableHandler : QRCmdHandlerBase
    {
        private new AvaliableRequest _request =>(AvaliableRequest)base._request;
        private new AvaliableResult _result =>(AvaliableResult)base._result;
        public AvailableHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            //prefix check
            for (int i = 0; i < AvaliableRequest.Prefix.Length; i++)
            {
                if (_request.RawRequest[i] != AvaliableRequest.Prefix[i])
                {
                    _result.ErrorCode = QRErrorCode.Parse;
                    return;
                }
            }

            //postfix check
            if (_request.RawRequest[_request.RawRequest.Length - 1] != AvaliableRequest.Postfix)
            {
                _result.ErrorCode = QRErrorCode.Parse;
                return;
            }
        }
        protected override void DataOperation()
        {
        }

        protected override void ResponseConstruct()
        {
            _response = new AvaliableResponse(_request,_result);
        }
    }
}
