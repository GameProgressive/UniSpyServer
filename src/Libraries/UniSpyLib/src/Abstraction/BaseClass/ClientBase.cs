using System;
using System.Collections.Generic;
using System.Net;
using System.Timers;
using StackExchange.Redis;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Config;
using UniSpyServer.UniSpyLib.Encryption;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass
{
    public abstract class ClientBase : IClient, ITestClient, IDisposable
    {
        public static IConnectionMultiplexer RedisConnection { get; set; }
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
            RedisConnection = ConnectionMultiplexer.Connect(ConfigManager.Config.Redis.ConnectionString);
            ClientPool = new Dictionary<IPEndPoint, IClient>();
        }

        public ClientBase(IConnection connection)
        {
            Connection = connection;
            EventBinding();
            lock (ClientPool)
            {
                // we fire this when there are no record in Sessions
                if (!ClientPool.ContainsKey(connection.RemoteIPEndPoint))
                {
                    ClientPool.Add(Connection.RemoteIPEndPoint, this);
                }
            }
        }
        private void EventBinding()
        {
            switch (Connection.ConnectionType)
            {
                case NetworkConnectionType.Tcp:
                    ((ITcpConnection)Connection).OnReceive += OnReceived;
                    ((ITcpConnection)Connection).OnConnect += OnConnected;
                    ((ITcpConnection)Connection).OnDisconnect += OnDisconnected;
                    break;
                case NetworkConnectionType.Udp:
                    ((IUdpConnection)Connection).OnReceive += OnReceived;
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
                    ((IHttpConnection)Connection).OnReceive += OnReceived;
                    ((IHttpConnection)Connection).OnConnect += OnConnected;
                    ((IHttpConnection)Connection).OnDisconnect += OnDisconnected;
                    break;
                case NetworkConnectionType.Test:
                    LogWriter.Info("Using unit-test proxy");
                    break;
                default:
                    throw new Exception("Unsupported connection type");
            }
        }
        /// <summary>
        /// Only work for tcp
        /// </summary>
        protected virtual void OnConnected()
        {
            lock (ClientPool)
            {
                if (!ClientPool.ContainsKey(Connection.RemoteIPEndPoint))
                {
                    ClientPool.TryAdd(Connection.RemoteIPEndPoint, this);
                }
            }
        }
        /// <summary>
        /// Only work for tcp
        /// </summary>
        protected virtual void OnDisconnected()
        {
            lock (ClientPool)
            {
                if (ClientPool.ContainsKey(Connection.RemoteIPEndPoint))
                {
                    ClientPool.Remove(Connection.RemoteIPEndPoint);
                }
            }
            Dispose();
        }

        protected abstract ISwitcher CreateSwitcher(object buffer);
        protected virtual void OnReceived(object buffer)
        {
            switch (Connection.ConnectionType)
            {
                case NetworkConnectionType.Tcp:
                    goto default;
                case NetworkConnectionType.Udp:
                    // reset timer for udp connection
                    lock (_timer)
                    {
                        _timer.Stop();
                        _timer.Start();
                    }
                    goto default;
                case NetworkConnectionType.Http:
                    LogNetworkReceiving(Connection.RemoteIPEndPoint, ((IHttpRequest)buffer).Body);
                    break;
                case NetworkConnectionType.Test:
                    goto default;
                default:
                    buffer = DecryptMessage((byte[])buffer);
                    LogNetworkReceiving(Connection.RemoteIPEndPoint, (byte[])buffer);
                    break;
            }
            // we let child class to create swicher for us
            var switcher = CreateSwitcher(buffer);
            switcher.Switch();
        }
        protected virtual byte[] DecryptMessage(byte[] buffer)
        {
            if (Crypto is not null)
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
            if (((IUdpConnection)Connection).ConnectionExistedTime > _expireTimeInterval)
            {
                ClientPool.Remove(((IUdpConnection)Connection).RemoteIPEndPoint);
                Dispose();
            }
        }

        public void Dispose()
        {
            switch (Connection.ConnectionType)
            {
                case NetworkConnectionType.Tcp:
                    ((ITcpConnection)Connection).OnReceive -= OnReceived;
                    ((ITcpConnection)Connection).OnConnect -= OnConnected;
                    ((ITcpConnection)Connection).OnDisconnect -= OnDisconnected;
                    break;
                case NetworkConnectionType.Udp:
                    ((IUdpConnection)Connection).OnReceive -= OnReceived;
                    _timer.Elapsed -= (s, e) => CheckExpiredClient();
                    _timer.Dispose();
                    break;
                case NetworkConnectionType.Http:
                    ((IHttpConnection)Connection).OnReceive -= OnReceived;
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
                buffer = UniSpyEncoding.GetBytes((string)response.SendingBuffer);
            }
            else
            {
                buffer = (byte[])response.SendingBuffer;
            }
            // LogWriter.LogNetworkSending(Session.RemoteIPEndPoint, buffer);
            LogNetworkSending(Connection.RemoteIPEndPoint, buffer);
            //Encrypt the response if Crypto is not null
            if (Crypto is not null)
            {
                buffer = Crypto.Encrypt(buffer);
            }
            Connection.Send(buffer);
        }
        protected virtual void LogNetworkSending(IPEndPoint ipEndPoint, object buffer)
        {
            var bufferType = buffer.GetType();
            if (bufferType == typeof(byte[]))
            {
                LogWriter.LogNetworkSending(ipEndPoint, (byte[])buffer);
                return;
            }
            if (bufferType == typeof(string))
            {
                LogWriter.LogNetworkSending(ipEndPoint, (string)buffer);
            }

        }
        protected virtual void LogNetworkReceiving(IPEndPoint ipEndPoint, object buffer)
        {
            var bufferType = buffer.GetType();
            if (bufferType == typeof(byte[]))
            {
                LogWriter.LogNetworkReceiving(ipEndPoint, (byte[])buffer);
                return;
            }
            if (bufferType == typeof(string))
            {
                LogWriter.LogNetworkReceiving(ipEndPoint, (string)buffer);
            }
        }
        /// <summary>
        /// Received function for unit-test
        /// </summary>
        /// <param name="buffer">Raw byte array</param>
        void ITestClient.TestReceived(byte[] buffer) => OnReceived(buffer);
    }
}
