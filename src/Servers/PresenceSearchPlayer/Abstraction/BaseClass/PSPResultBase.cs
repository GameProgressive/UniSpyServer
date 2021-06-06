using PresenceSearchPlayer.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Abstraction.BaseClass
{
    public abstract class PSPResultBase : UniSpyResultBase
    {
        public new GPErrorCode ErrorCode
        {
            get => (GPErrorCode)base.ErrorCode;
            set => base.ErrorCode = value;
        }

        public PSPResultBase()
        {
            ErrorCode = GPErrorCode.NoError;
        }
    }
}
