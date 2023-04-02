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
        protected UniSpyServerConfig _cfg;
        public string Name { get; } = _name;
        /// <summary>
        /// This name is for reading from child class, indicating the server name
        /// </summary>
        protected static string _name;

        public ServerBase()
        {
            SetServerInfo();
            ConnectionManager = CreateConnectionManager(ListeningIPEndPoint);
            ConnectionManager.OnInitialization += HandleConnectionInitialization;
        }
        /// <summary>
        /// This constructor is for unittests
        /// </summary>
        public ServerBase(IConnectionManager manager)
        {
            SetServerInfo();
            ConnectionManager = manager;
        }
        public void SetServerInfo()
        {
            _cfg = ConfigManager.Config.Servers.Where(s => s.ServerName == _name).First();
            Id = _cfg.ServerID;
            ListeningIPEndPoint = _cfg.ListeningIPEndPoint;
            PublicIPEndPoint = _cfg.PublicIPEndPoint;
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