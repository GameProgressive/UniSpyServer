using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;


namespace CDKey.Handler.CommandSwitcher
{
    public class CDKeyCommandSwitcher : CommandSwitcherBase
    {
        protected new string _rawRequest;
        public CDKeyCommandSwitcher(ISession session, object rawRequest) : base(session, rawRequest)
        {
            _rawRequest = (string)rawRequest;
        }

        protected override void SerializeCommands()
        {
            foreach (var request in _requests)
            {
                _handlers.Add(new CDKeyCommandHandlerSerializer(_session, request).Serialize());
            }
        }

        protected override void SerializeRequests()
        {
            string[] commands = _rawRequest.Split(@"\r\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var command in commands)
            {
                var request = new CDKeyRequestSerializer(command).Serialize();
                if (!(bool)request.Parse())
                {
                    return;
                }

                _requests.Add(request);
            }
        }
    }
}
