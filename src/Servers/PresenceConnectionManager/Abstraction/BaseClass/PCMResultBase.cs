using PresenceSearchPlayer.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Abstraction.BaseClass
{
    public abstract class PCMResultBase : UniSpyResultBase
    {
        public new GPErrorCode ErrorCode
        {
            get { return (GPErrorCode)base.ErrorCode; }
            set { base.ErrorCode = value; }
        }
        public new PCMRequestBase Request
        {
            get { return (PCMRequestBase)base.Request; }
        }
        public PCMResultBase()
        {
        }

        protected PCMResultBase(UniSpyRequestBase request) : base(request)
        {
            ErrorCode = GPErrorCode.NoError;
        }
    }
}
