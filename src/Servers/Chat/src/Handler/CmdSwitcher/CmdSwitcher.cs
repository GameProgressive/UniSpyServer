using UniSpyServer.Servers.Chat.Network;
using System;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CommandSwitcher
{
    /// <summary>
    /// Process request to Commands
    /// </summary>
    public sealed class CmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new string _rawRequest => (string)base._rawRequest;
        private new Session _session => (Session)base._session;
        public CmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void ProcessRawRequest()
        {
            string[] splitedRawRequests = _rawRequest.Replace("\r", "")
                   .Split("\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var rawRequest in splitedRawRequests)
            {
                var name = rawRequest.Trim(' ').Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList().First();
                _cmdMapping.Add(name, rawRequest);
            }
        }
    }
}
