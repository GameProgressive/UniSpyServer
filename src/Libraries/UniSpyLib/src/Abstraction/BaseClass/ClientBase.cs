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
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass
{
    public abstract class ClientBase : IClient, IDisposable
    {
        public static ConnectionMultiplexer Redis { get; set; }
        public static IDictionary<IPEndPoint, IClient> ClientPool { get; private set; }
        public ISession Session { get; private set; }
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

        public ClientBase(ISession session)
        {
            Session = session;
            EventBinding();
        }
        private void EventBinding()
        {
            if (Session.GetType() == typeof(UdpSession))
            {
                ((UdpSession)Session).OnReceive += OnReceived;
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
            else if (Session.GetType() == typeof(TcpSession))
            {
                ((TcpSession)Session).OnReceive += OnReceived;
                ((TcpSession)Session).OnConnect += OnConnected;
                ((TcpSession)Session).OnDisconnect += OnDisconnected;
            }
            else if (Session.GetType() == typeof(HttpSession))
            {
                ((HttpSession)Session).OnReceive += OnReceived;
            }
            else
            {
                throw new System.Exception("Unsupported session type");
            }
        }
        public static ClientBase CreateClient(ISession session)
        {
            var clientType = Assembly.GetEntryAssembly().GetType($"UniSpyServer.Servers.{session.Server.ServerName}.Entity.Structure.Client");
            var client = (ClientBase)Activator.CreateInstance(clientType, new object[] { session });
            return client;
        }
        protected virtual void OnConnected()
        {
            if (!ClientPool.ContainsKey(Session.RemoteIPEndPoint))
            {
                ClientPool.TryAdd(Session.RemoteIPEndPoint, this);
            }
        }
        protected virtual void OnDisconnected()
        {
            if (ClientPool.ContainsKey(Session.RemoteIPEndPoint))
            {
                ClientPool.Remove(Session.RemoteIPEndPoint);
            }
            Dispose();
        }
        protected virtual void OnReceived(object buffer)
        {
            if (!ClientPool.ContainsKey(Session.RemoteIPEndPoint))
            {
                ClientPool.TryAdd(Session.RemoteIPEndPoint, this);
            }
            // reset timer for udp session
            if (Session.GetType() == typeof(UdpSession))
            {
                _timer.Stop();
                _timer.Start();
            }
            buffer = DecryptMessage((byte[])buffer);
            LogWriter.LogNetworkReceiving(Session.RemoteIPEndPoint, (byte[])buffer);
            // create switcher instance by reflection
            var switcherType = Assembly.GetEntryAssembly().GetType($"UniSpyServer.Servers.{Session.Server.ServerName}.Handler.CmdSwitcher");
            var switcher = (ISwitcher)Activator.CreateInstance(switcherType, new object[] { this, buffer });
            switcher.Switch();
        }

        protected virtual byte[] DecryptMessage(byte[] buffer)
        {
            if (Crypto != null)
            {
                return Crypto.Decrypt(buffer);
            }
            else
            {
                return buffer;
            }
        }
        /// <summary>
        /// Check if the udp client is expired,
        /// this method will only be invoked by UdpServer
        /// </summary>
        private void CheckExpiredClient()
        {
            // we calculate the interval between last packe and current time
            if (((UdpSession)Session).SessionExistedTime > _expireTimeInterval)
            {
                ClientPool.Remove(((UdpSession)Session).RemoteIPEndPoint);
                Dispose();
            }
        }

        public void Dispose()
        {
            if (Session.GetType() == typeof(UdpSession))
            {
                ((UdpSession)Session).OnReceive -= OnReceived;
                _timer.Elapsed -= (s, e) => CheckExpiredClient();
            }
            else if (Session.GetType() == typeof(TcpSession))
            {
                ((TcpSession)Session).OnReceive -= OnReceived;
                ((TcpSession)Session).OnConnect -= OnConnected;
                ((TcpSession)Session).OnDisconnect -= OnDisconnected;
            }
            else if (Session.GetType() == typeof(HttpSession))
            {
                ((HttpSession)Session).OnReceive -= OnReceived;
                ((HttpSession)Session).OnConnect -= OnConnected;
                ((HttpSession)Session).OnDisconnect -= OnDisconnected;
            }
        }
    }
}
