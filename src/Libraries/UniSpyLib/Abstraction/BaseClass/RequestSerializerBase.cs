using System.Collections.Generic;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class RequestSerializerBase
    {
        protected ISession _session;
        protected object _rawRequest;
        public List<IRequest> Requests;
        public RequestSerializerBase(ISession session, object rawRequest)
        {
            _session = session;
            _rawRequest = rawRequest;
            Requests = new List<IRequest>();
            LogWriter.LogCurrentClass(this);
        }

        public abstract void Serialize();

        protected abstract IRequest GenerateRequest(object singleRequest);
    }
}
