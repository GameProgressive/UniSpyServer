using GameSpyLib.Common.BaseClass;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;
using QueryReport.Entity.Enumerator;
using QueryReport.Handler.SystemHandler.ErrorMessage;
using Serilog.Events;

namespace QueryReport.Handler.CommandHandler
{
    public class QRCommandHandlerBase : CommandHandlerBase
    {
        protected QRErrorCode _errorCode = QRErrorCode.NoError;
        protected byte[] _sendingBuffer;
        protected byte[] _recv;

        protected QRCommandHandlerBase(ISession session, byte[] recv) : base(session)
        {
            _recv = recv;
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
