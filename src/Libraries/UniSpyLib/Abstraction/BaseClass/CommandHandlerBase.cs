using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class CommandHandlerBase
    {
        protected ISession _session;
        public CommandHandlerBase(ISession session)
        {
            _session = session;
        }

        public virtual void Handle()
        {
            LogWriter.LogCurrentClass(this);
        }
    }
}
