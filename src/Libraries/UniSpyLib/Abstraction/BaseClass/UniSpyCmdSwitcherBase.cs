using System;
using System.Collections.Generic;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyCmdSwitcherBase
    {
        protected object _rawRequest;
        protected IUniSpySession _session;
        protected List<IUniSpyRequest> _requests;
        protected List<IUniSpyHandler> _handlers;
        public UniSpyCmdSwitcherBase(IUniSpySession session, object rawRequest)
        {
            _session = session;
            _rawRequest = rawRequest;
            _requests = new List<IUniSpyRequest>();
            _handlers = new List<IUniSpyHandler>();
            LogWriter.LogCurrentClass(this);
        }

        public virtual void Switch()
        {
            try
            {
                DeserializeRequests();
                if (_requests.Count == 0)
                {
                    return;
                }
                DeserializeCmdHandlers();
                if (_handlers.Count == 0)
                {
                    return;
                }

                // TODO changes foreach to parallel foreach
                foreach (var handler in _handlers)
                {
                    handler.Handle();
                }
            }
            catch (UniSpyExceptionBase e)
            {
                LogWriter.ToLog(e);
            }
        }

        protected abstract void DeserializeRequests();
        protected virtual void DeserializeCmdHandlers()
        {
            foreach (var request in _requests)
            {
                var handler = (IUniSpyHandler)Activator.CreateInstance(
                    UniSpyServerFactoryBase.HandlerMapping[(string)request.CommandName],
                    _session, request);

                if (handler == null)
                {
                    return;
                }
                _handlers.Add(handler);
            }
        }
    }
}
