using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Handler.CommandHandler;
using UniSpyLib.Abstraction.BaseClass;

namespace NATNegotiation.Handler.CommandSwitcher
{
    public class NNCommandSwitcher : CommandSwitcherBase
    {
        protected new byte[] _rawRequest;
        public NNCommandSwitcher(ISession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void SerializeCommandHandlers()
        {
            foreach (var request in _requests)
            {
                var handler = new NNCommandHandlerSerializer(_session, request).Serialize();
                if (handler == null)
                {
                    return;
                }

                _handlers.Add(handler);
            }
        }

        protected override void SerializeRequests()
        {
            var request = new NNRequestSerializer(_rawRequest).Serialize();
            if (!(bool)request.Parse())
            {
                return;
            }

            _requests.Add(request);
        }
    }
}
