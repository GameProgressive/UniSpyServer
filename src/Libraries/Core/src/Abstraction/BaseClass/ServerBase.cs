using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Config;

namespace UniSpy.Server.Core.Abstraction.BaseClass
{
    /// <summary>
    /// Represent UniSpy server instance abstraction
    /// </summary>
    public abstract class ServerBase : IServer
    {
        public Guid Id { get; private set; }
        public IPEndPoint ListeningIPEndPoint { get; private set; }
        public IPEndPoint PublicIPEndPoint { get; private set; }
        [JsonIgnore]
        public IConnectionManager ConnectionManager;
        string IServer.Name => _name;
        public static string Name
        {
            get
            {
                if (_name is null)
                {
                    throw new UniSpyException("Server Name must set in child class.");
                }
                else
                {
                    return _name;
                }
            }
        }
        protected static string _name;

        public ServerBase()
        {
            var cfg = ConfigManager.Config.Servers.Where(s => s.ServerName == Name).First();
            Id = cfg.ServerID;
            ListeningIPEndPoint = cfg.ListeningIPEndPoint;
            PublicIPEndPoint = cfg.PublicIPEndPoint;
            ConnectionManager = CreateConnectionManager(ListeningIPEndPoint);
            ConnectionManager.OnInitialization += HandleConnectionInitialization;
        }
        /// <summary>
        /// This constructor is for unittests
        /// </summary>
        public ServerBase(IConnectionManager manager)
        {
            ConnectionManager = manager;
            var cfg = ConfigManager.Config.Servers.Where(s => s.ServerName == Name).First();
            Id = cfg.ServerID;
            ListeningIPEndPoint = cfg.ListeningIPEndPoint;
            PublicIPEndPoint = cfg.PublicIPEndPoint;
        }

        protected abstract IConnectionManager CreateConnectionManager(IPEndPoint endPoint);
        private IClient HandleConnectionInitialization(IConnection connection)
        {
            var client = ClientManagerBase.GetClient(connection.RemoteIPEndPoint);
            if (client is null)
            {
                client = CreateClient(connection);
                ClientManagerBase.AddClient(client);
            }
            return client;
        }
        protected abstract IClient CreateClient(IConnection connection);

        public virtual void Start() => ConnectionManager.Start();
    }
}