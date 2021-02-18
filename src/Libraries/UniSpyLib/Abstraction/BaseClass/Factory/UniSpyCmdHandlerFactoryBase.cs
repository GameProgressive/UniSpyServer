using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyCmdHandlerFactoryBase
    {
        protected IUniSpyRequest _request;
        protected IUniSpySession _session;
        public UniSpyCmdHandlerFactoryBase(IUniSpySession session, IUniSpyRequest request)
        {
            _request = request;
            _session = session;
            LogWriter.LogCurrentClass(this);
        }

        public abstract IUniSpyHandler Serialize();
    }
}
