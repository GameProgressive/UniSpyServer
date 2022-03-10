using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Timers;
using StackExchange.Redis;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Application.Network.Udp.Server;
using UniSpyServer.UniSpyLib.Config;
using UniSpyServer.UniSpyLib.Encryption;
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
        private static Type _clientType;
        private static Type _switcherType;

        static ClientBase()
        {
            if (Redis is null)
            {
                Redis = ConnectionMultiplexer.Connect(ConfigManager.Config.Redis.ConnectionString);
            }
            if (ClientPool is null)
            {
                ClientPool = new ConcurrentDictionary<IPEndPoint, IClient>();
            }
        }

        public ClientBase(ISession session)
        {
            Session = session;
            EventBinding();
        }
        private void EventBinding()
        {
            switch (Session.ConnectionType)
            {
                case NetworkConnectionType.Tcp:
                    ((ITcpSession)Session).OnReceive += OnReceived;
                    ((ITcpSession)Session).OnConnect += OnConnected;
                    ((ITcpSession)Session).OnDisconnect += OnDisconnected;
                    break;
                case NetworkConnectionType.Udp:
                    ((IUdpSession)Session).OnReceive += OnReceived;
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
                    break;
                case NetworkConnectionType.Http:
                    ((IHttpSession)Session).OnReceive += OnReceived;
                    ((IHttpSession)Session).OnConnect += OnConnected;
                    ((IHttpSession)Session).OnDisconnect += OnDisconnected;
                    break;
                case NetworkConnectionType.Test:
                    LogWriter.Info("Using unit-test proxy");
                    break;
                default:
                    throw new Exception("Unsupported session type");
            }
        }
        public static ClientBase CreateClient(ISession session)
        {
            if (_clientType is null)
            {
                _clientType = Assembly.GetEntryAssembly().GetType($"UniSpyServer.Servers.{session.Server.ServerName}.Entity.Structure.Client");
            }

            // create client and bind client with session
            var client = (ClientBase)Activator.CreateInstance(_clientType, new object[] { session });
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
            switch (Session.ConnectionType)
            {
                case NetworkConnectionType.Tcp:
                    goto default;
                case NetworkConnectionType.Udp:
                    // reset timer for udp session
                    _timer.Stop();
                    _timer.Start();
                    goto default;
                case NetworkConnectionType.Http:
                    LogWriter.LogNetworkReceiving(Session.RemoteIPEndPoint, ((NetCoreServer.HttpRequest)buffer).Body);
                    break;
                case NetworkConnectionType.Test:
                    goto default;
                default:
                    buffer = DecryptMessage((byte[])buffer);
                    LogWriter.LogNetworkReceiving(Session.RemoteIPEndPoint, (byte[])buffer);
                    break;
            }
            // create switcher instance by reflection
            if (_switcherType is null)
            {
                _switcherType = Assembly.GetEntryAssembly().GetType($"UniSpyServer.Servers.{Session.Server.ServerName}.Handler.CmdSwitcher");
            }
            var switcherParams = new object[] { this, buffer };
            var switcher = (ISwitcher)Activator.CreateInstance(_switcherType, switcherParams);
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
            switch (Session.ConnectionType)
            {
                case NetworkConnectionType.Tcp:
                    ((ITcpSession)Session).OnReceive -= OnReceived;
                    ((ITcpSession)Session).OnConnect -= OnConnected;
                    ((ITcpSession)Session).OnDisconnect -= OnDisconnected;
                    break;
                case NetworkConnectionType.Udp:
                    ((IUdpSession)Session).OnReceive -= OnReceived;
                    _timer.Elapsed -= (s, e) => CheckExpiredClient();
                    _timer.Dispose();
                    break;
                case NetworkConnectionType.Http:
                    ((IHttpSession)Session).OnReceive -= OnReceived;
                    break;
            }
        }
        /// <summary>
        /// Sending IResponse to client(ciphertext or plaintext)
        /// </summary>
        /// <param name="response"></param>
        public void Send(IResponse response)
        {
            byte[] buffer = null;
            response.Build();
            if (response.SendingBuffer.GetType() == typeof(string))
            {
                LogWriter.LogNetworkSending(Session.RemoteIPEndPoint, (string)response.SendingBuffer);
                buffer = UniSpyEncoding.GetBytes((string)response.SendingBuffer);
            }
            else
            {
                buffer = (byte[])response.SendingBuffer;
                LogWriter.LogNetworkSending(Session.RemoteIPEndPoint, (byte[])buffer);

            }

            //Encrypt the response if Crypto is not null
            if (Crypto is not null)
            {
                buffer = Crypto.Encrypt(buffer);
            }
            Session.Send(buffer);
        }
    }
}
