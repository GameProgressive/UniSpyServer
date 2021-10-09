using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyResultBase
    {
        public UniSpyResultBase()
        {
            LogWriter.LogCurrentClass(this);
        }
    }
}
