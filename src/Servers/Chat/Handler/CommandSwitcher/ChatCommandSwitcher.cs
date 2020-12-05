using UniSpyLib.Abstraction.Interface;
using System;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Handler.CommandSwitcher
{
    /// <summary>
    /// Process request to Commands
    /// </summary>
    public class ChatCommandSwitcher : CommandSwitcherBase
    {
        protected new string _rawRequest;
        public ChatCommandSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void SerializeCommandHandlers()
        {
            foreach (var request in _requests)
            {
                _handlers.Add(new ChatCommandHandlerSerializer(_session, request).Serialize());
            }
        }

        protected override void SerializeRequests()
        {
            string[] rawRequests = _rawRequest.Replace("\r", "")
                   .Split("\n", StringSplitOptions.RemoveEmptyEntries);
            // first we convert request into our ChatCommand class
            // next we handle each command
            foreach (var rawRequest in rawRequests)
            {
                _requests.Add(new ChatRequestSerializer(rawRequest).Serialize());
            }
        }
    }
}
