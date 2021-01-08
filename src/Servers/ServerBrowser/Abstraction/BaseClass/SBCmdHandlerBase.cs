using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using Serilog.Events;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Handler.SystemHandler.Error;
using ServerBrowser.Network;

namespace ServerBrowser.Abstraction.BaseClass
{
    public abstract class SBCmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected SBErrorCode _errorCode;
        protected byte[] _sendingBuffer;
        protected new SBSession _session { get { return (SBSession)base._session; } }

        public SBCmdHandlerBase(IUniSpySession session, IUniSpyRequest request):base(session,request)
        {
            _errorCode = SBErrorCode.NoError;
        }

        public override void Handle()
        {
            RequestCheck();

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

            ResponseConstruct();

            if (_errorCode != SBErrorCode.NoError)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.GetErrorMsg(_errorCode));
                return;
            }

            Response();
        }

        protected override void Response()
        {
            if (_sendingBuffer == null || _sendingBuffer.Length < 4)
            {
                return;
            }
            _session.SendAsync(_sendingBuffer);
        }
    }
}
