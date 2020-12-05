using System.Collections.Generic;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class CommandSwitcherBase
    {
        protected IUniSpySession _session;
        protected object _rawRequest;
        protected List<IUniSpyRequest> _requests;
        protected List<IUniSpyHandler> _handlers;
        public CommandSwitcherBase(IUniSpySession session, object rawRequest)
        {
            _session = session;
            _rawRequest = rawRequest;
            _requests = new List<IUniSpyRequest>();
            _handlers = new List<IUniSpyHandler>();
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
