using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyResultBase
    {
        public UniSpyResultBase()
        {
            LogWriter.LogCurrentClass(this);
        }
    }
}
