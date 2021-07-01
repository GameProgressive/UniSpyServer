using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyRequest : IUniSpyRequest
    {
        public object CommandName { get; protected set; }
        public object RawRequest { get; protected set; }
        public UniSpyRequest() { }
        public UniSpyRequest(object rawRequest)
        {
            RawRequest = rawRequest;
            LogWriter.LogCurrentClass(this);
        }

        public abstract void Parse();
    }
}
