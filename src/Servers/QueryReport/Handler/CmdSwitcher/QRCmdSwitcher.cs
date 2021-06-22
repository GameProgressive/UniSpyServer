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

        protected override void DeserializeCmdHandlers()
        {
            foreach (var request in _requests)
            {
                _handlers.Add(new QRCmdHandlerFactory(_session, request).Deserialize());
            }
        }

        protected override void DeserializeRequests()
        {
            var request = new QRRequestFactory(_rawRequest).Deserialize();
            request.Parse();
            _requests.Add(request);
        }
    }
}
