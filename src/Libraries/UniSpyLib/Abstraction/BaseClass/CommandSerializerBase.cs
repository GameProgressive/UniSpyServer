using System.Collections.Generic;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class CommandSerializerBase
    {
        protected ISession _session;
        protected object _rawRequest;
        protected List<IRequest> Requests;
        public CommandSerializerBase(ISession session, object rawRequest)
        {
            _session = session;
            _rawRequest = rawRequest;
            Requests = new List<IRequest>();
            LogWriter.LogCurrentClass(this);
        }

        public abstract void Serialize();

        public virtual void Handle()
        {
            SerializeRequests();
            SerializeCommands();
        }
        protected virtual void SerializeRequests() { }
        protected virtual void SerializeCommands() { }
    }
}
