using System.Collections.Generic;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.Core.Abstraction.BaseClass
{
    public abstract class CmdSwitcherBase : ISwitcher
    {
        protected IClient _client;
        protected object _rawRequest { get; private set; }
        protected List<KeyValuePair<object, object>> _requests { get; private set; }
        protected List<IHandler> _handlers { get; private set; }

        public CmdSwitcherBase(IClient client, object rawRequest)
        {
            _client = client;
            _rawRequest = rawRequest;
            _handlers = new List<IHandler>();
            _requests = new List<KeyValuePair<object, object>>();
        }

        public virtual void Handle()
        {
            try
            {
                //First we process the message split it to raw requests.
                ProcessRawRequest();
                // Then we process the raw requests to UniSpy requests.
                if (_requests.Count == 0)
                {
                    return;
                }
                // Then we create UniSpy handlers by UniSpy requests.
                foreach (var rawRequest in _requests)
                {
                    var handler = CreateCmdHandlers(rawRequest.Key, rawRequest.Value);
                    if (handler is null)
                    {
                        _client.LogWarn("Request ignored.");
                        continue;
                    }
                    _handlers.Add(handler);
                }
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
                _client.LogError(e.Message);
            }
        }
        /// <summary>
        /// Split the raw requests into single raw request
        /// get the request type and create dictionary of (request type, request)
        /// </summary>
        /// <returns>request type, single request</returns>
        protected abstract void ProcessRawRequest();
        /// <summary>
        /// Call this in size 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rawRequest"></param>
        protected abstract IHandler CreateCmdHandlers(object name, object rawRequest);
    }
}
