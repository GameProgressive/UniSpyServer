using System;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.CDkey.Handler.CmdSwitcher
{
    public sealed class CDKeyCmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new string _rawRequest => (string)base._rawRequest;

        public CDKeyCmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void ProcessRawRequest()
        {
            string[] commands = _rawRequest.Split(@"\r\n", StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
