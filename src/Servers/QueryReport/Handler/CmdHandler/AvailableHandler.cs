using System.Linq;
using UniSpyLib.Abstraction.Interface;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;
using QueryReport.Entity.Structure.Request;
using QueryReport.Entity.Structure.Response;

namespace QueryReport.Handler.CmdHandler
{
    /// <summary>
    /// AvailableCheckHandler
    /// </summary>
    public class AvailableHandler : QRCmdHandlerBase
    {
        protected new AvaliableRequest _request { get { return (AvaliableRequest)base._request; } }
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
                    _errorCode = QRErrorCode.Parse;
                    return;
                }
            }

            //postfix check
            if (_request.RawRequest[_request.RawRequest.Length - 1] != AvaliableRequest.Postfix)
            {
                _errorCode = QRErrorCode.Parse;
                return;
            }
        }

        protected override void ResponseConstruct()
        {
            _sendingBuffer = new AvaliableResponse().BuildResponse();
        }
    }
}
