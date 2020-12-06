using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyCmdHandlerBase : IUniSpyHandler
    {
        protected IUniSpySession _session;
        protected IUniSpyRequest _request;
        public UniSpyCmdHandlerBase(IUniSpySession session,IUniSpyRequest request)
        {
            _session = session;
            _request = request;
        }

        public virtual void Handle()
        {
            LogWriter.LogCurrentClass(this);
        }

        void IUniSpyHandler.Handle() => Handle();
    }
}
