using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass
{
    public abstract class ResultBase
    {
        public ResultBase()
        {
            LogWriter.LogCurrentClass(this);
        }
    }
}
