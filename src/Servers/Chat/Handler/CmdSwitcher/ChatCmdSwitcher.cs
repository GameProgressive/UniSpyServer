using Chat.Network;
using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CommandSwitcher
{
    /// <summary>
    /// Process request to Commands
    /// </summary>
    internal sealed class ChatCmdSwitcher : UniSpyCmdSwitcher
    {
        private new string _rawRequest
        {
            get => (string)base._rawRequest;
            set => base._rawRequest = value;
        }
        private new ChatSession _session => (ChatSession)base._session;
        public ChatCmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void DeserializeCmdHandlers()
        {
            foreach (var request in _requests)
            {
                _handlers.Add(new ChatCmdHandlerFactory(_session, request).Deserialize());
            }
        }

        protected override void DeserializeRequests()
        {
            string[] rawRequests = _rawRequest.Replace("\r", "")
                   .Split("\n", StringSplitOptions.RemoveEmptyEntries);
            // first we convert request into our ChatCommand class
            // next we handle each command
            foreach (var rawRequest in rawRequests)
            {
                var request = new ChatRequestFacotry(rawRequest).Deserialize();
                try
                {
                    request.Parse();
                }
                catch
                {
                    continue;
                }
                _requests.Add(request);
            }
        }
    }
}
