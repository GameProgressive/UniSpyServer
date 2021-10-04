using System;
using System.Text;
using NatNegotiation.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyLib.Abstraction.Interface;

namespace NatNegotiation.Handler.CmdSwitcher
{
    internal sealed class CmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new byte[] _rawRequest => (byte[])base._rawRequest;
        public CmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }
        protected override void ProcessRawRequest()
        {
            var name = (RequestType)_rawRequest[7];
            _cmdMapping.Add(name, _rawRequest);
        }
    }
}
