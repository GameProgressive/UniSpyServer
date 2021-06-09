using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace GameStatus.Handler.CmdSwitcher
{
    internal sealed class GSCmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new string _rawRequest
        {
            get => (string)base._rawRequest;
            set => base._rawRequest = value;
        }
        public GSCmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void SerializeCommandHandlers()
        {
            foreach (var request in _requests)
            {
                var handler = new GSCmdHandlerFactory(_session, request).Serialize();
                if (handler == null)
                {
                    return;
                }
                _handlers.Add(handler);
            }
        }

        protected override void SerializeRequests()
        {
            var request = new GSRequestFactory(_rawRequest).Serialize();
            request.Parse();
            _requests.Add(request);
        }
    }
}
