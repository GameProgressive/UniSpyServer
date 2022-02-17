using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Timers;
using StackExchange.Redis;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Application.Network.Http.Server;
using UniSpyServer.UniSpyLib.Application.Network.Tcp.Server;
using UniSpyServer.UniSpyLib.Application.Network.Udp.Server;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass
{
    public abstract class ClientBase : IClient, IDisposable
    {
        public static ConnectionMultiplexer Redis { get; set; }
        public static IDictionary<IPEndPoint, IClient> ClientPool { get; private set; }
        public IConnection Connection { get; private set; }
        public ICryptography Crypto { get; set; }
        public ClientInfoBase Info { get; protected set; }
        /// <summary>
        /// The time interval that client remove from ClientPool
        /// </summary>
        /// <value></value>
        protected TimeSpan _expireTimeInterval { get; set; }
        /// <summary>
        /// The timer to count and invoke some event
        /// </summary>
        private Timer _timer;
        static ClientBase()
        {
            Redis = ConnectionMultiplexer.Connect(ConfigManager.Config.Redis.ConnectionString);
            ClientPool = new ConcurrentDictionary<IPEndPoint, IClient>();
        }

        public ClientBase(IConnection connection)
        {
            Connection = connection;
        }
        private void EventBinding()
        {
            if (Connection.GetType() == typeof(UdpSession))
            {
                ((UdpSession)Connection).OnReceive += OnReceived;
                // todo add timer here
                _timer = new Timer
                {
                    Enabled = true,
                    Interval = 60000,
                    AutoReset = true
                };//10000
                // we set expire time to 1 hour
                _expireTimeInterval = new TimeSpan(1, 0, 0);
                _timer.Start();
                _timer.Elapsed += (s, e) => CheckExpiredClient();
            }
            else if (Connection.GetType() == typeof(TcpSession))
            {
                ((TcpSession)Connection).OnReceive += OnReceived;
                ((TcpSession)Connection).OnConnect += OnConnected;
                ((TcpSession)Connection).OnDisconnect += OnDisconnected;
            }
            else if (Connection.GetType() == typeof(HttpSession))
            {
                ((HttpSession)Connection).OnReceive += OnReceived;
            }
            else
            {
                throw new System.Exception("Unsupported session type");
            }
            if (!ClientPool.ContainsKey(Connection.RemoteIPEndPoint))
            {
                ClientPool.Add(Connection.RemoteIPEndPoint, this);
            }
        }
        public static ClientBase CreateClient(IConnection session)
        {
            var n = Assembly.GetEntryAssembly().GetName().Name;
            var clientType = Assembly.GetEntryAssembly().GetType($"UniSpyServer.Servers.{session.Server.ServerName}.Entity.Structure.Client");
            var client = (ClientBase)Activator.CreateInstance(clientType, new object[] { session });
            return client;
        }
        protected virtual void OnConnected()
        {
            if (!ClientPool.ContainsKey(Connection.RemoteIPEndPoint))
            {
                ClientPool.Add(Connection.RemoteIPEndPoint, this);
            }
        }
        protected virtual void OnDisconnected()
        {
            if (ClientPool.ContainsKey(Connection.RemoteIPEndPoint))
            {
                ClientPool.Remove(Connection.RemoteIPEndPoint);
            }
        }
        protected virtual void OnReceived(object buffer)
        {
            if (!ClientPool.ContainsKey(Connection.RemoteIPEndPoint))
            {
                ClientPool.Add(Connection.RemoteIPEndPoint, this);
            }
            // reset timer for udp session
            if (Connection.GetType() == typeof(UdpSession))
            {
                _timer.Stop();
                _timer.Start();
            }
        }
        /// <summary>
        /// Check if the udp client is expired,
        /// this method will only be invoked by UdpServer
        /// </summary>
        private void CheckExpiredClient()
        {
            // we calculate the interval between last packe and current time
            if (((UdpSession)Connection).SessionExistedTime > _expireTimeInterval)
            {
                ClientPool.Remove(((UdpSession)Connection).RemoteIPEndPoint);
                Dispose();
            }
        }

        public void Dispose()
        {
            if (Connection.GetType() == typeof(UdpSession))
            {
                ((UdpSession)Connection).OnReceive -= OnReceived;
                _timer.Elapsed -= (s, e) => CheckExpiredClient();
            }
            else if (Connection.GetType() == typeof(TcpSession))
            {
                ((TcpSession)Connection).OnReceive -= OnReceived;
                ((TcpSession)Connection).OnConnect -= OnConnected;
                ((TcpSession)Connection).OnDisconnect -= OnDisconnected;
            }
            else if (Connection.GetType() == typeof(HttpSession))
            {
                ((HttpSession)Connection).OnReceive -= OnReceived;
                ((HttpSession)Connection).OnConnect -= OnConnected;
                ((HttpSession)Connection).OnDisconnect -= OnDisconnected;
            }
        }
    }
}
