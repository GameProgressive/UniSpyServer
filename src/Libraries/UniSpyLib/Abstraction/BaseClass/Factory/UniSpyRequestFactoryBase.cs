using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyRequestFactoryBase
    {
        protected object _rawRequest;
        public UniSpyRequestFactoryBase(object rawRequest)
        {
            _rawRequest = rawRequest;
            LogWriter.LogCurrentClass(this);
        }

        public abstract IUniSpyRequest Serialize();
    }
}
