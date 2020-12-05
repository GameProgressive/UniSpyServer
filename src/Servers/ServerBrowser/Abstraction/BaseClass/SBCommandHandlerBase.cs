using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using Serilog.Events;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Handler.SystemHandler.Error;
using ServerBrowser.Network;

namespace ServerBrowser.Abstraction.BaseClass
{
    public abstract class SBCommandHandlerBase : CommandHandlerBase
    {
        protected SBErrorCode _errorCode;
        protected byte[] _sendingBuffer;
        protected new SBSession _session;
        protected IUniSpyRequest _request;

        public SBCommandHandlerBase(IUniSpySession session, IUniSpyRequest request):base(session)
        {
            _errorCode = SBErrorCode.NoError;
            _request = request;
            _session = (SBSession)session;
        }

        public override void Handle()
        {
            base.Handle();

            CheckRequest();

            if (_errorCode != SBErrorCode.NoError)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.GetErrorMsg(_errorCode));
                return;
            }

            DataOperation();

            if (_errorCode != SBErrorCode.NoError)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.GetErrorMsg(_errorCode));
                return;
            }

            ConstructResponse();

            if (_errorCode != SBErrorCode.NoError)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.GetErrorMsg(_errorCode));
                return;
            }

            Response();
        }

        protected virtual void CheckRequest()
        { }

        protected virtual void DataOperation()
        { }

        protected virtual void ConstructResponse()
        { }

        protected virtual void Response()
        {
            if (_sendingBuffer == null || _sendingBuffer.Length < 4)
            {
                return;
            }
            _session.SendAsync(_sendingBuffer);
        }
    }
}
