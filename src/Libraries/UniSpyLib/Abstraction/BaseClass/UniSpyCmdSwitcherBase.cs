using Serilog.Events;
using System;
using System.Collections.Generic;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyCmdSwitcherBase
    {
        protected IUniSpySession _session;
        protected object _rawRequest;
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
                // we first decrypt raw request and log it
                Decrypt();
                LogNetworkTraffic();
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

                // TODO changes foreach to parallel foreach
                foreach (var handler in _handlers)
                {
                    handler.Handle();
                }
            }
            catch (Exception e)
            {
                LogWriter.ToLog(LogEventLevel.Error, e.ToString());
            }
        }

        protected abstract void SerializeRequests();
        protected abstract void SerializeCommandHandlers();

        /// <summary>
        /// Decrypt message and log decrypted message
        /// </summary>
        protected virtual void Decrypt() { }
        private void LogNetworkTraffic()
        {
            if (_rawRequest.GetType().Equals(typeof(string)))
            {
                LogWriter.LogNetworkReceiving(_session.RemoteIPEndPoint, (string)_rawRequest);
            }
            else if (_rawRequest.GetType().Equals(typeof(byte[])))
            {
                LogWriter.LogNetworkReceiving(_session.RemoteIPEndPoint, (byte[])_rawRequest);
            }
            else
            {
                throw new FormatException("_rawRequest is an unknown type!");
            }
        }
    }
}
