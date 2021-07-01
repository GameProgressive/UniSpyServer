using System;
using System.Collections.Generic;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass.Factory;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyCmdSwitcher
    {
        protected object _rawRequest;
        protected IUniSpySession _session;
        protected List<IUniSpyRequest> _requests;
        protected List<IUniSpyHandler> _handlers;
        public UniSpyCmdSwitcher(IUniSpySession session, object rawRequest)
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
            catch (UniSpyException e)
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
                    UniSpyServerFactory.HandlerMapping[(byte)request.CommandName],
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
