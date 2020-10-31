using GameSpyLib.Abstraction.BaseClass;
using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Logging;
using QueryReport.Entity.Enumerate;
using QueryReport.Handler.SystemHandler.ErrorMessage;
using QueryReport.Server;
using Serilog.Events;

namespace QueryReport.Abstraction.BaseClass
{
    public class QRCommandHandlerBase : CommandHandlerBase
    {
        protected QRErrorCode _errorCode = QRErrorCode.NoError;
        protected byte[] _sendingBuffer;
        protected byte[] _recv;
        protected new QRSession _session;
        protected QRCommandHandlerBase(ISession session, byte[] rawRequest) : base(session)
        {
            _recv = rawRequest;
            _session = (QRSession)session;
        }

        public override void Handle()
        {
            base.Handle();

            CheckRequest();

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

            ConstructeResponse();

            if (_errorCode != QRErrorCode.NoError)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.GetErrorMessage(_errorCode));
                return;
            }

            Response();
        }

        protected virtual void CheckRequest() { }

        protected virtual void DataOperation() { }

        protected virtual void ConstructeResponse() { }

        protected virtual void Response()
        {
            if (_sendingBuffer == null)
            {
                return;
            }

            _session.SendAsync(_sendingBuffer);
        }
    }
}
