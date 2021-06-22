using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace NATNegotiation.Handler.CmdSwitcher
{
    internal sealed class NNCmdSwitcher : UniSpyCmdSwitcherBase
    {
        public NNCmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void SerializeCommandHandlers()
        {
            foreach (var request in _requests)
            {
                var handler = new NNCmdHandlerFactory(_session, request).Serialize();
                if (handler == null)
                {
                    return;
                }

                _handlers.Add(handler);
            }
        }

        protected override void SerializeRequests()
        {
            var request = new NNRequestFactory(_rawRequest).Deserialize();
            request.Parse();
            _requests.Add(request);
        }
    }
}
