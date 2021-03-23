using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;


namespace CDKey.Handler.CmdSwitcher
{
    internal sealed class CDKeyCmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new string _rawRequest => (string)base._rawRequest;
        public CDKeyCmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void SerializeCommandHandlers()
        {
            foreach (var request in _requests)
            {
                _handlers.Add(new CDKeyCmdHandlerFactory(_session, request).Serialize());
            }
        }

        protected override void SerializeRequests()
        {
            string[] commands = _rawRequest.Split(@"\r\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var command in commands)
            {
                var request = new CDKeyRequestFactory(command).Serialize();
                request.Parse();

                if (!(bool)request.ErrorCode)
                {
                    return;
                }

                _requests.Add(request);
            }
        }
    }
}
