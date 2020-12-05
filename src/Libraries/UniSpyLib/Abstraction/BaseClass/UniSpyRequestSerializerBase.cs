using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyRequestSerializerBase
    {
        protected object _rawRequest;
        public UniSpyRequestSerializerBase(object rawRequest)
        {
            _rawRequest = rawRequest;
            LogWriter.LogCurrentClass(this);
        }

        public abstract IUniSpyRequest Serialize();
    }
}
