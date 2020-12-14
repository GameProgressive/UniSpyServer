using QueryReport.Entity.Enumerate;
using QueryReport.Handler.SystemHandler.ErrorMessage;
using Serilog.Events;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace QueryReport.Handler.CmdSwitcher
{
    public class QRCmdSwitcher : UniSpyCmdSwitcherBase
    {
        protected new byte[] _rawRequest
        {
            get { return (byte[])base._rawRequest; }
        }
        public QRCmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void SerializeCommandHandlers()
        {
            foreach (var request in _requests)
            {
                _handlers.Add(new QRCmdHandlerSerializer(_session, request).Serialize());
            }
        }

        protected override void SerializeRequests()
        {
            var request = new QRRequestSerializer(_rawRequest).Serialize();
            request.Parse();
            if (!(bool)request.ErrorCode)
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.GetErrorMessage(QRErrorCode.Parse));
                return;
            }
            _requests.Add(request);
        }
    }
}
