using System;
using QueryReport.Entity.Enumerate;
using QueryReport.Handler.SystemHandler.ErrorMessage;
using Serilog.Events;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace QueryReport.Handler.CommandSwitcher
{
    public class QRCommandSwitcher:CommandSwitcherBase
    {
        protected new byte[] _rawRequest;
        public QRCommandSwitcher(ISession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void SerializeCommandHandlers()
        {
            foreach (var request in _requests)
            {
                _handlers.Add(new QRCommandHandlerSerializer(_session, request).Serialize());
            }
        }

        protected override void SerializeRequests()
        {
            var request = new QRRequestSerializer(_rawRequest).Serialize();
            if (!(bool)request.Parse())
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.GetErrorMessage(QRErrorCode.Parse));
                return;
            }
            _requests.Add(request);
        }
    }
}
