using GameStatus.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace GameStatus.Abstraction.BaseClass
{
    internal abstract class GSResultBase : UniSpyResultBase
    {
        public new GSErrorCode ErrorCode
        {
            get => (GSErrorCode)base.ErrorCode;
            set => base.ErrorCode = value;
        }
        public GSResultBase()
        {
            ErrorCode = GSErrorCode.NoError;
        }
    }
}
