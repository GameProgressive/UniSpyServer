using System;
using System.Text;
using GameStatus.Entity.Enumerate;
using GameStatus.Entity.Structure.Misc;
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
            if ((GSErrorCode)request.ErrorCode != GSErrorCode.NoError)
            {
                return;
            }
            _requests.Add(request);
        }
        protected override void Decrypt()
        {
            byte[] buffer = Encoding.ASCII.GetBytes(_rawRequest);
            _rawRequest = Encoding.ASCII.GetString(GSEncryption.Decrypt(buffer));
        }
    }
}
