using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace CDKey.Handler.CmdSwitcher
{
    internal sealed class CDKeyCmdSwitcher : UniSpyCmdSwitcherBase
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
