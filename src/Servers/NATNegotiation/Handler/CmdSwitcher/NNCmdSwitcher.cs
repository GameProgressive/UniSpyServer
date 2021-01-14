using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace NATNegotiation.Handler.CmdSwitcher
{
    public class NNCmdSwitcher : UniSpyCmdSwitcherBase
    {
        protected new byte[] _rawRequest { get { return (byte[])base._rawRequest; } }
        public NNCmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void SerializeCommandHandlers()
        {
            foreach (var request in _requests)
            {
                var handler = new NNCmdHandlerSerializer(_session, request).Serialize();
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
            request.Parse();
            if (!(bool)request.ErrorCode)
            {
                return;
            }

            _requests.Add(request);
        }
    }
}
