using System.Collections.Generic;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class CommandSwitcherBase
    {
        protected ISession _session;
        protected object _rawRequest;
        protected List<IRequest> _requests;
        protected List<IHandler> _handlers;
        public CommandSwitcherBase(ISession session, object rawRequest)
        {
            _session = session;
            _rawRequest = rawRequest;
            _requests = new List<IRequest>();
            _handlers = new List<IHandler>();
            LogWriter.LogCurrentClass(this);
        }

        public virtual void Switch()
        {
            SerializeRequests();
            if (_requests.Count == 0)
            {
                return;
            }
            SerializeCommandHandlers();
            if (_handlers.Count == 0)
            {
                return;
            }

            foreach (var handler in _handlers)
            {
                handler.Handle();
            }
        }

        protected abstract void SerializeRequests();
        protected abstract void SerializeCommandHandlers();
    }
}
