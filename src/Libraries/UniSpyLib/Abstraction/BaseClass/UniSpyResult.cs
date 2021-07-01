using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyResult
    {
        public UniSpyResult()
        {
            LogWriter.LogCurrentClass(this);
        }
    }
}
