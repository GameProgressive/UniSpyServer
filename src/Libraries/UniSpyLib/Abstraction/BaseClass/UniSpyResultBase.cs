using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyResultBase
    {
        public object ErrorCode { get; set; }
        public UniSpyRequestBase Request { get; protected set; }
        public UniSpyResultBase() { }
        public UniSpyResultBase(UniSpyRequestBase request)
        {
            Request = request;
            LogWriter.LogCurrentClass(this);
        }
    }
}
