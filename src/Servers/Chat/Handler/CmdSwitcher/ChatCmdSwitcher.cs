using System;
using System.Text;
using Chat.Entity.Structure;
using Chat.Entity.Structure.Misc;
using Chat.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Encryption;

namespace Chat.Handler.CommandSwitcher
{
    /// <summary>
    /// Process request to Commands
    /// </summary>
    internal sealed class ChatCmdSwitcher : UniSpyCmdSwitcherBase
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

        protected override void SerializeCommandHandlers()
        {
            foreach (var request in _requests)
            {
                _handlers.Add(new ChatCmdHandlerFactory(_session, request).Serialize());
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
                var request = new ChatRequestFacotry(rawRequest).Serialize();
                request.Parse();
                if ((ChatErrorCode)request.ErrorCode != ChatErrorCode.NoError)
                {
                    break;
                }
                _requests.Add(request);
            }
        }
    }
}
