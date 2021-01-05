using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyResultBase
    {
        public object ErrorCode { get; set; }
        public UniSpyResultBase()
        {
            LogWriter.LogCurrentClass(this);
        }
    }
}
