using Chat.Network;
using System;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CommandSwitcher
{
    /// <summary>
    /// Process request to Commands
    /// </summary>
    internal sealed class CmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new string _message { get => (string)base._message; set => base._message = value; }
        private new Session _session => (Session)base._session;
        public CmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void ProcessRawRequest()
        {
            string[] splitedRawRequests = _message.Replace("\r", "")
                   .Split("\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var rawRequest in splitedRawRequests)
            {
                var name = rawRequest.Trim(' ').Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList().First();
                _rawRequests.Add(name, rawRequest);
            }
        }
    }
}
