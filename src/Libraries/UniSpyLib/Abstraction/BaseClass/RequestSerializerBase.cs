using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class RequestSerializerBase
    {
        protected object _rawRequest;
        public RequestSerializerBase(object rawRequest)
        {
            _rawRequest = rawRequest;
            LogWriter.LogCurrentClass(this);
        }

        public abstract IRequest Serialize();
    }
}
