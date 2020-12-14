using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Abstraction.BaseClass;

namespace ServerBrowser.Handler.CommandSwitcher
{
    public class SBCmdSwitcher : UniSpyCmdSwitcherBase
    {
        protected new byte[] _rawRequest
        {
            get { return (byte[])base._rawRequest; }
            set { base._rawRequest = value; }
        }
        public SBCmdSwitcher(IUniSpySession session, byte[] rawRequest) : base(session, rawRequest)
        {
        }

        protected override void SerializeCommandHandlers()
        {
            foreach (var request in _requests)
            {
                var handler = new SBCmdHandlerSerializer(_session, request).Serialize();
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
            request.Parse();
            if (!(bool)request.ErrorCode)
            {
                return;
            }
            _requests.Add(request);
        }
    }
}
