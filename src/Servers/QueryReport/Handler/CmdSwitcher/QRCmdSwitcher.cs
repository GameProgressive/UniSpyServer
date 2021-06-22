using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace QueryReport.Handler.CmdSwitcher
{
    internal sealed class QRCmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new byte[] _rawRequest => (byte[])base._rawRequest;
        public QRCmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void SerializeCommandHandlers()
        {
            foreach (var request in _requests)
            {
                _handlers.Add(new QRCmdHandlerFactory(_session, request).Serialize());
            }
        }

        protected override void SerializeRequests()
        {
            var request = new QRRequestFactory(_rawRequest).Deserialize();
            request.Parse();
            _requests.Add(request);
        }
    }
}
