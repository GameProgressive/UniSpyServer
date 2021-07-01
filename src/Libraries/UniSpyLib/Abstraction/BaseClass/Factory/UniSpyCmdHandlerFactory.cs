using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyCmdHandlerFactory
    {
        protected IUniSpyRequest _request;
        protected IUniSpySession _session;
        public UniSpyCmdHandlerFactory(IUniSpySession session, IUniSpyRequest request)
        {
            _request = request;
            _session = session;
            LogWriter.LogCurrentClass(this);
        }
        public abstract IUniSpyHandler Deserialize();
    }
}
