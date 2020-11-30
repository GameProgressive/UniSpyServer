using System.Linq;
using UniSpyLib.Abstraction.Interface;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;
using QueryReport.Entity.Structure.Request;
using QueryReport.Entity.Structure.Response;

namespace QueryReport.Handler.CommandHandler
{
    /// <summary>
    /// AvailableCheckHandler
    /// </summary>
    public class AvailableHandler : QRCommandHandlerBase
    {
        protected new AvaliableRequest _request;
        public AvailableHandler(ISession session, IRequest request) : base(session, request)
        {
            _request = (AvaliableRequest)request;
        }

        protected override void CheckRequest()
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

        protected override void ConstructeResponse()
        {
            _sendingBuffer = new AvaliableResponse().BuildResponse();
        }
    }
}
