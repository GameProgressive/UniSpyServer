using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using UniSpyServer.UniSpyLib.Abstraction.Contract;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass
{
    public abstract class CmdSwitcherBase
    {
        // !! new architecture
        /// <summary>
        /// <IPEndPoint|IClient>
        /// </summary>
        public static IDictionary<IPEndPoint, IClient> ClientPool { get; private set; }
        protected object _rawRequest { get; private set; }
        protected ISession _session { get; private set; }
        protected List<IRequest> _requests { get; private set; }
        protected List<IHandler> _handlers { get; private set; }
        /// <summary>
        /// The gamespy requests and UniSpy requets mapping dictionary
        /// </summary>
        protected static Dictionary<object, Type> _requestMapping { get; set; }
        protected static Dictionary<object, Type> _handlerMapping { get; set; }
        static CmdSwitcherBase()
        {
            _requestMapping = LoadUniSpyComponents(typeof(RequestContractBase));
            _handlerMapping = LoadUniSpyComponents(typeof(HandlerContractBase));
            ClientPool = new ConcurrentDictionary<IPEndPoint, IClient>();
        }
        public CmdSwitcherBase(ISession session, object rawRequest)
        {
            _session = session;
            _rawRequest = rawRequest;
            _requests = new List<IRequest>();
            _handlers = new List<IHandler>();
            // if (ClientPool.ContainsKey(_session.RemoteIPEndPoint))
            // {
            //     _client = ClientPool[_session.RemoteIPEndPoint];
            // }
            // else
            // {
            //     _client = new Client(_session);
            //     ClientPool.Add(_session.RemoteIPEndPoint, _client);
            // }
        }

        protected CmdSwitcherBase()
        {
        }

        public static Dictionary<object, Type> LoadUniSpyComponents(Type type)
        {
            var mapping = new Dictionary<object, Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(a => a.GetTypes().Where(t => t.IsDefined(type, false))).ToList();

            if (assemblies.Count() == 0)
            {
                throw new NotImplementedException("Components have not been implemented");
            }

            foreach (var assembly in assemblies)
            {
                var attr = assembly.GetCustomAttributes<CmdContractBase>().FirstOrDefault<CmdContractBase>();

                if (attr == null)
                    continue;

                if (mapping.ContainsKey(attr.Name))
                {
                    throw new ArgumentException($"Duplicate commands {attr.Name} for type {assembly.FullName}");
                }
                mapping.Add(attr.Name, assembly);
            }
            return mapping;
        }

        public virtual void Switch()
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
                CreateCmdHandlers();
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
                LogWriter.Error(e.Message);
            }
        }
        /// <summary>
        /// Split the raw requests into single raw request
        /// get the request type and create dictionary of (request type, request)
        /// </summary>
        /// <returns>request type, single request</returns>
        protected abstract void ProcessRawRequest();
        protected virtual void DeserializeRequest(object name, object rawRequest)
        {
            if (!_requestMapping.ContainsKey(name))
            {
                LogWriter.Error($"request {name} is not implemented");
            }
            _requests.Add((IRequest)Activator.CreateInstance(_requestMapping[name], rawRequest));
        }
        protected virtual void CreateCmdHandlers()
        {
            foreach (var request in _requests)
            {
                var requestName = request.GetType().GetCustomAttribute<RequestContractBase>().Name;
                if (!_handlerMapping.ContainsKey(requestName))
                {
                    LogWriter.Error($"Handler {requestName} is not implemented");

                }
                var handler = (IHandler)Activator.CreateInstance(_handlerMapping[requestName], _session, request);

                if (handler == null)
                {
                    return;
                }
                _handlers.Add(handler);
            }
        }
    }
}
