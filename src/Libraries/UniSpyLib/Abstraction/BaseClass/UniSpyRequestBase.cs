using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyRequestBase : IUniSpyRequest
    {
        public object CommandName { get; protected set; }
        public object RawRequest { get; protected set; }
        public object ErrorCode { get; protected set; }
        public UniSpyRequestBase(){}
        public UniSpyRequestBase(object rawRequest)
        {
            RawRequest = rawRequest;
            LogWriter.LogCurrentClass(this);
        }

        public abstract void Parse();
    }
}
