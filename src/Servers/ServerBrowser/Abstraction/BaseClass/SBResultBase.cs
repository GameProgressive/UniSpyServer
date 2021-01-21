using System;
using ServerBrowser.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace ServerBrowser.Abstraction.BaseClass
{
    public class SBResultBase:UniSpyResultBase
    {
        public new SBErrorCode ErrorCode
        {
            get => (SBErrorCode)base.ErrorCode;
            set => base.ErrorCode = value;
        }

        public SBResultBase()
        {
        }
    }
}
