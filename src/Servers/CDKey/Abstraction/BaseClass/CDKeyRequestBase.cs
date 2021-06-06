using CDKey.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace CDKey.Abstraction.BaseClass
{
    internal abstract class CDKeyRequestBase : UniSpyRequestBase
    {
        public new CDKeyErrorCode ErrorCode
        {
            get => (CDKeyErrorCode)base.ErrorCode;
            protected set => base.ErrorCode = value;
        }
        public CDKeyRequestBase(string rawRequest) : base(rawRequest)
        {
            ErrorCode = CDKeyErrorCode.NoError;
        }

        public override void Parse()
        {
        }
    }
}
