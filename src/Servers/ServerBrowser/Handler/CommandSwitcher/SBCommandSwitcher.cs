using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Abstraction.BaseClass;

namespace ServerBrowser.Handler.CommandSwitcher
{
    public class SBCommandSwitcher : CommandSwitcherBase
    {
        protected new byte[] _rawRequest;
        public SBCommandSwitcher(IUniSpySession session, byte[] rawRequest) : base(session, rawRequest)
        {
            _rawRequest = rawRequest;
        }

        protected override void SerializeCommandHandlers()
        {
            foreach (var request in _requests)
            {
                var handler = new SBCommandHandlerSerializer(_session, request).Serialize();
                if (handler == null)
                {
                    continue;
                }

                _handlers.Add(handler);
            }
        }

        protected override void SerializeRequests()
        {
            var request = new SBRequestSerializer(_rawRequest).Serialize();
            if (!(bool)request.Parse())
            {
                return;
            }
            _requests.Add(request);
        }
    }
}
