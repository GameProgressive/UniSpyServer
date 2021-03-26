using ServerBrowser.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace ServerBrowser.Handler.CommandSwitcher
{
    internal sealed class SBCmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new byte[] _rawRequest => (byte[])base._rawRequest;

        public SBCmdSwitcher(IUniSpySession session, byte[] rawRequest) : base(session, rawRequest)
        {
        }

        protected override void SerializeCommandHandlers()
        {
            foreach (var request in _requests)
            {
                var handler = new SBCmdHandlerFactory(_session, request).Serialize();
                if (handler == null)
                {
                    continue;
                }

                _handlers.Add(handler);
            }
        }

        protected override void SerializeRequests()
        {
            var request = new SBRequestFactory(_rawRequest).Serialize();
            request.Parse();
            if ((SBErrorCode)request.ErrorCode != SBErrorCode.NoError)
            {
                return;
            }
            _requests.Add(request);
        }
    }
}
