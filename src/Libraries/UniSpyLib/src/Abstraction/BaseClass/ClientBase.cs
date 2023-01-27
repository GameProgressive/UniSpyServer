using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.Net;
using System.Timers;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Encryption;
using UniSpyServer.UniSpyLib.Extensions;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass
{
    public abstract class ClientBase : IClient, ITestClient, IDisposable
    {
        public static readonly IDictionary<IPEndPoint, IClient> ClientPool = new ConcurrentDictionary<IPEndPoint, IClient>();
        public IConnection Connection { get; private set; }
        public ICryptography Crypto { get; set; }
        public ClientInfoBase Info { get; protected set; }
        /// <summary>
        /// The time interval that client remove from ClientPool
        /// </summary>
        /// <value></value>
        protected TimeSpan _expireTimeInterval { get; set; }
        protected DateTime LastPacketReceivedTime { get; set; }
        public TimeSpan ConnectionExistedTime => DateTime.Now - LastPacketReceivedTime;
        /// <summary>
        /// The timer to count and invoke some event
        /// </summary>
        private Timer _timer;
        protected bool _isLogRawMessage;

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
                    LastPacketReceivedTime = DateTime.Now;
                    _expireTimeInterval = new TimeSpan(1, 0, 0);
                    _timer.Start();
                    _timer.Elapsed += (s, e) => CheckExpiredClient();
                    break;
                case NetworkConnectionType.Http:
                    ((IHttpConnection)Connection).OnReceive += OnReceived;
                    ((IHttpConnection)Connection).OnConnect += OnConnected;
                    ((IHttpConnection)Connection).OnDisconnect += OnDisconnected;
                    break;
                // case NetworkConnectionType.Test:
                //     LogInfo("Using unit-test mock connection.");
                //     break;
                default:
                    throw new Exception("Unsupported connection type.");
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
        /// <summary>
        /// Invoked when received message from game client
        /// </summary>
        /// <param name="buffer">Byte[], string, Http</param>
        protected virtual void OnReceived(object buffer)
        {
            switch (Connection.ConnectionType)
            {
                case NetworkConnectionType.Tcp:
                    goto default;
                case NetworkConnectionType.Udp:
                    LastPacketReceivedTime = DateTime.Now;
                    goto default;
                case NetworkConnectionType.Http:
                    var tempBuffer = ((IHttpRequest)buffer).Body;
                    this.LogNetworkReceiving(tempBuffer);
                    break;
                default:
                    buffer = DecryptMessage((byte[])buffer);
                    this.LogNetworkReceiving((byte[])buffer, _isLogRawMessage);
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
                if (ClientPool.TryGetValue(((IUdpConnection)Connection).RemoteIPEndPoint, out _))
                {
                    ClientPool.Remove(((IUdpConnection)Connection).RemoteIPEndPoint);
                }
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
            this.LogNetworkSending(buffer, _isLogRawMessage);
            //Encrypt the response if Crypto is not null
            if (Crypto is not null)
            {
                buffer = Crypto.Encrypt(buffer);
            }
            Connection.Send(buffer);
        }
        /// <summary>
        /// Received function for unit-test
        /// </summary>
        /// <param name="buffer">Raw byte array</param>
        void ITestClient.TestReceived(byte[] buffer) => OnReceived(buffer);
    }
}
