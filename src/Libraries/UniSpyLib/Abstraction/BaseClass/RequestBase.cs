using System;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyRequestBase : IRequest
    {
        public object CommandName { get; protected set; }
        public object RawRequest { get; protected set; }

        public UniSpyRequestBase(object rawRequest)
        {
            RawRequest = rawRequest;
            LogWriter.LogCurrentClass(this);
        }

        public abstract object Parse();
    }
}
