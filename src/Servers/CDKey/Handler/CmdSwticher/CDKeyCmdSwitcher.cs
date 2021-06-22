using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace CDKey.Handler.CmdSwitcher
{
    internal sealed class CDKeyCmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new string _rawRequest
        {
            get => (string)base._rawRequest;
            set => base._rawRequest = value;
        }
        public CDKeyCmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void DeserializeCmdHandlers()
        {
            foreach (var request in _requests)
            {
                _handlers.Add(new CDKeyCmdHandlerFactory(_session, request).Deserialize());
            }
        }

        protected override void DeserializeRequests()
        {
            string[] commands = _rawRequest.Split(@"\r\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var command in commands)
            {
                var request = new CDKeyRequestFactory(command).Deserialize();
                request.Parse();
                _requests.Add(request);
            }
        }
    }
}
