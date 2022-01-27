using System;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Handler.CmdSwitcher
{
    public sealed class CmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new string _rawRequest => (string)base._rawRequest;
        public CmdSwitcher(IUniSpySession session, string rawRequest) : base(session, rawRequest)
        {
        }
        protected override void ProcessRawRequest()
        {
            if (_rawRequest[0] != '\\')
            {
                LogWriter.Info("Invalid request recieved!");
                return;
            }
            string[] rawRequests = _rawRequest.Split("\\final\\", StringSplitOptions.RemoveEmptyEntries);
            foreach (var rawRequest in rawRequests)
            {
                var name = rawRequest.TrimStart('\\').Split("\\").First();
                _cmdMapping.Add(name, rawRequest);
            }
        }
    }
}
