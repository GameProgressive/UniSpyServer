using System;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class RequestBase:IRequest
    {
        public object CommandName { get; protected set; }
        public object RawRequest { get; protected set; }

        public RequestBase()
        {
            LogWriter.LogCurrentClass(this);
        }

        public virtual object Parse()
        {
            throw new NotImplementedException();
        }
    }
}
