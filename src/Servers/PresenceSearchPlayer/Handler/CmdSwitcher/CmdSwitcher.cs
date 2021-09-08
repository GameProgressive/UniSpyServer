using PresenceSearchPlayer.Entity.Exception.General;
using System;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace PresenceSearchPlayer.Handler.CmdSwitcher
{
    internal sealed class CmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new string _message => (string)base._message;
        public CmdSwitcher(IUniSpySession session, string rawRequest) : base(session, rawRequest)
        {
        }
        protected override void ProcessRawRequest()
        {
            if (_message[0] != '\\')
            {
                LogWriter.Info("Invalid request recieved!");
                return;
            }
            string[] rawRequests = _message.Split("\\final\\", StringSplitOptions.RemoveEmptyEntries);
            foreach (var rawRequest in rawRequests)
            {
                var name = rawRequest.TrimStart('\\').Split("\\").First();
                _rawRequests.Add(name, rawRequest);
            }
        }
    }
}
