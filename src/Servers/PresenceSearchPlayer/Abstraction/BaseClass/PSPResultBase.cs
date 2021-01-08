using PresenceSearchPlayer.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Abstraction.BaseClass
{
    public abstract class PSPResultBase : UniSpyResultBase
    {
        public new GPErrorCode ErrorCode
        {
            get { return (GPErrorCode)base.ErrorCode; }
            set { base.ErrorCode = value; }
        }
        public new PSPRequestBase Request
        {
            get { return (PSPRequestBase)base.Request; }
            set { base.Request = value; }
        }

        public PSPResultBase() { }

        public PSPResultBase(UniSpyRequestBase request) : base(request)
        {
            ErrorCode = GPErrorCode.NoError;
        }
    }
}
