using PresenceSearchPlayer.Entity.Exception.General;
using System;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using UniSpyLib.MiscMethod;

namespace PresenceConnectionManager.Handler.CommandSwitcher
{
    internal sealed class CmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new string _rawRequest => (string)base._rawRequest;
        public CmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }
        protected override void ProcessRawRequest()
        {
            if (_rawRequest[0] != '\\')
            {
                throw new GPParseException("Request format is invalid");
            }
            var rawRequests = _rawRequest.Split(@"\final\", StringSplitOptions.RemoveEmptyEntries);

            foreach (var rawRequest in rawRequests)
            {
                var name = rawRequest.TrimStart('\\').Split('\\').First();
                _cmdMapping.Add(name, rawRequest);
            }
        }
    }
}