using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Logging;

namespace GameSpyLib.Abstraction.BaseClass
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
