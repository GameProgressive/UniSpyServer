using CDKey.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace CDKey.Abstraction.BaseClass
{
    internal abstract class CDKeyResultBase : UniSpyResultBase
    {
        public new CDKeyErrorCode ErrorCode
        {
            get { return (CDKeyErrorCode)base.ErrorCode; }
            set { base.ErrorCode = value; }
        }
        protected CDKeyResultBase()
        {
        }
    }
}
