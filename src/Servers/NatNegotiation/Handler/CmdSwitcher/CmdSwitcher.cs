using System;
using NatNegotiation.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyLib.Abstraction.Interface;

namespace NatNegotiation.Handler.CmdSwitcher
{
    internal sealed class CmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new byte[] _message => (byte[])base._message;
        public CmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }
        protected override void ProcessRawRequest()
        {
            var name = (RequestType)_message[7];
            _rawRequests.Add(name, _message);
        }
    }
}
