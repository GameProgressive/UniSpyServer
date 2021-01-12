using NATNegotiation.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace NATNegotiation.Abstraction.BaseClass
{
    internal abstract class NNResultBase : UniSpyResultBase
    {
        public new NNErrorCode ErrorCode
        {
            get { return (NNErrorCode)base.ErrorCode; }
            protected set { base.ErrorCode = value; }
        }
        protected NNResultBase()
        {
            ErrorCode = NNErrorCode.NoError;
        }
    }
}
