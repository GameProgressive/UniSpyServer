using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class CommandHandlerBase : IUniSpyHandler
    {
        protected IUniSpySession _session;
        public CommandHandlerBase(IUniSpySession session)
        {
            _session = session;
        }

        public virtual void Handle()
        {
            LogWriter.LogCurrentClass(this);
        }

        void IUniSpyHandler.Handle() => Handle();
    }
}
