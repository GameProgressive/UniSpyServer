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

        protected override void DeserializeCmdHandlers()
        {
            foreach (var request in _requests)
            {
                var handler = new SBCmdHandlerFactory(_session, request).Deserialize();
                if (handler == null)
                {
                    continue;
                }

                _handlers.Add(handler);
            }
        }

        protected override void DeserializeRequests()
        {
            var request = new SBRequestFactory(_rawRequest).Deserialize();
            request.Parse();
            _requests.Add(request);
        }
    }
}
