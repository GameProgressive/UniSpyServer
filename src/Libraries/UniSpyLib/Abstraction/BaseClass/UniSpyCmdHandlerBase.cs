using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyCmdHandlerBase : IUniSpyHandler
    {
        protected IUniSpySession _session;
        protected IUniSpyRequest _request;
        protected IUniSpyResponse _response;
        protected UniSpyResultBase _result;
        //protected object _sendingBuffer;
        public UniSpyCmdHandlerBase(IUniSpySession session, IUniSpyRequest request)
        {
            _session = session;
            _request = request;
            LogWriter.LogCurrentClass(this);
        }

        public abstract void Handle();


        protected abstract void CheckRequest();
        protected abstract void DataOperation();
        protected abstract void ConstructResponse();
        protected abstract void Response();
    }
}
