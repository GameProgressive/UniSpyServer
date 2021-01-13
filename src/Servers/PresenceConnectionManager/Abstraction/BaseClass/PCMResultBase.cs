using PresenceSearchPlayer.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Abstraction.BaseClass
{
    internal abstract class PCMResultBase : UniSpyResultBase
    {
        public new GPErrorCode ErrorCode
        {
            get { return (GPErrorCode)base.ErrorCode; }
            set { base.ErrorCode = value; }
        }

        public PCMResultBase()
        {
            ErrorCode = GPErrorCode.NoError;
        }
    }
}
