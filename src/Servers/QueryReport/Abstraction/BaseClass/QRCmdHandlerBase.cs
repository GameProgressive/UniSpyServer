using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using QueryReport.Entity.Enumerate;
using QueryReport.Handler.SystemHandler.ErrorMessage;
using QueryReport.Network;
using Serilog.Events;

namespace QueryReport.Abstraction.BaseClass
{
    public class QRCmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected QRErrorCode _errorCode;
        protected byte[] _sendingBuffer;
        protected new QRSession _session
        {
            get { return (QRSession)base._session; }
        }
        protected QRCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session,request)
        {
            _request = request;
            _errorCode = QRErrorCode.NoError;
        }

        public override void Handle()
        {
            RequestCheck();

            if (_errorCode != QRErrorCode.NoError)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.GetErrorMessage(_errorCode));
                return;
            }

            DataOperation();

            if (_errorCode == QRErrorCode.Database)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.GetErrorMessage(_errorCode));
                return;
            }

            ResponseConstruct();

            if (_errorCode != QRErrorCode.NoError)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.GetErrorMessage(_errorCode));
                return;
            }

            Response();
        }


        protected override void Response()
        {
            if (_sendingBuffer == null)
            {
                return;
            }

            _session.SendAsync(_sendingBuffer);
        }
    }
}
