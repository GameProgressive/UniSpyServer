using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyCmdHandlerBase : IUniSpyHandler
    {
        protected IUniSpySession _session;
        protected IUniSpyRequest _request;
        protected IUniSpyResponse _response;
        //protected object _sendingBuffer;
        public UniSpyCmdHandlerBase(IUniSpySession session, IUniSpyRequest request)
        {
            _session = session;
            _request = request;
            LogWriter.LogCurrentClass(this);
        }

        public abstract void Handle();


        protected virtual void CheckRequest() { }
        protected virtual void DataOperation() { }
        protected virtual void ConstructResponse() { }
        protected virtual void Response() { }

        protected virtual void BuildErrorResponse() { }
        protected virtual void BuildNormalResponse() { }

    }
}
