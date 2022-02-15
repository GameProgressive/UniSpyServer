using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Timers;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Http.Server;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Udp.Server;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass
{
    public abstract class ClientBase : IClient
    {
        public static IDictionary<IPEndPoint, IClient> ClientPool { get; private set; }
        public ISession Session { get; private set; }
        public ICryptography Crypto { get; set; }
        public UserInfoBase UserInfo { get; protected set; }
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
            ClientPool = new ConcurrentDictionary<IPEndPoint, IClient>();
        }
        public ClientBase(ISession session, UserInfoBase userInfo)
        {
            Session = session;
            UserInfo = userInfo;

            if (session.GetType() == typeof(UniSpyUdpSession))
            {
                ((UniSpyUdpSession)session).OnReceive += OnReceived;
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
            else if (session.GetType() == typeof(UniSpyTcpSession))
            {
                ((UniSpyTcpSession)session).OnReceive += OnReceived;
                ((UniSpyTcpSession)session).OnConnect += OnConnected;
                ((UniSpyTcpSession)session).OnDisconnect += OnDisconnected;
            }
            else if (session.GetType() == typeof(UniSpyHttpSession))
            {
                ((UniSpyHttpSession)session).OnReceive += OnReceived;
                ((UniSpyHttpSession)session).OnConnect += OnConnected;
                ((UniSpyHttpSession)session).OnDisconnect += OnDisconnected;
            }
            else
            {
                throw new System.Exception("Unsupported session type");
            }
            if (!ClientPool.ContainsKey(Session.RemoteIPEndPoint))
            {
                ClientPool.Add(Session.RemoteIPEndPoint, this);
            }
        }
        protected virtual void OnConnected()
        {
            if (!ClientPool.ContainsKey(Session.RemoteIPEndPoint))
            {
                ClientPool.Add(Session.RemoteIPEndPoint, this);
            }
        }
        protected virtual void OnDisconnected()
        {
            if (ClientPool.ContainsKey(Session.RemoteIPEndPoint))
            {
                ClientPool.Remove(Session.RemoteIPEndPoint);
            }
        }
        protected virtual void OnReceived(object buffer)
        {
            if (!ClientPool.ContainsKey(Session.RemoteIPEndPoint))
            {
                ClientPool.Add(Session.RemoteIPEndPoint, this);
            }
        }
        /// <summary>
        /// Check if the udp client is expired,
        /// this method will only be invoked by UdpServer
        /// </summary>
        private void CheckExpiredClient()
        {
            // we calculate the interval between last packe and current time
            if (((UniSpyUdpSession)Session).SessionExistedTime > _expireTimeInterval)
            {
                ClientPool.Remove(((UniSpyUdpSession)Session).RemoteIPEndPoint);
            }
        }
    }
}