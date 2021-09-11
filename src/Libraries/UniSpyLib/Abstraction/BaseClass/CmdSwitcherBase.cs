using System;
using System.Collections.Generic;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using UniSpyLib.Abstraction.BaseClass.Contract;
using System.Linq;
using System.Reflection;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class CmdSwitcherBase
    {
        protected object _message;
        protected Dictionary<object, object> _rawRequests;
        protected IUniSpySession _session;
        protected List<IUniSpyRequest> _requests;
        protected List<IUniSpyHandler> _handlers;
        /// <summary>
        /// The gamespy requests and UniSpy requets mapping dictionary
        /// </summary>
        protected static Dictionary<object, Type> _requestMapping { get; private set; }
        /// <summary>
        /// The gamespy requests and UniSpy requets mapping dictionary
        /// </summary>
        protected static Dictionary<object, Type> _handlerMapping { get; private set; }
        static CmdSwitcherBase()
        {
            _requestMapping = LoadUniSpyComponents(typeof(RequestContractBase));
            _handlerMapping = LoadUniSpyComponents(typeof(HandlerContractBase));
        }
        public CmdSwitcherBase(IUniSpySession session, object rawRequest)
        {
            _session = session;
            _message = rawRequest;
            _requests = new List<IUniSpyRequest>();
            _handlers = new List<IUniSpyHandler>();
            _rawRequests = new Dictionary<object, object>();
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
                DeserializeRequests();
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
        protected virtual void DeserializeRequests()
        {
            foreach (var request in _rawRequests)
            {
                _requests.Add((IUniSpyRequest)Activator.CreateInstance(_requestMapping[request.Key], request.Value));
            }
        }
        protected virtual void CreateCmdHandlers()
        {
            foreach (var request in _requests)
            {
                var requestName = request.GetType().GetCustomAttribute<RequestContractBase>().Name;
                var handler = (IUniSpyHandler)Activator.CreateInstance(_handlerMapping[requestName], _session, request);

                if (handler == null)
                {
                    return;
                }
                _handlers.Add(handler);
            }
        }
    }
}
