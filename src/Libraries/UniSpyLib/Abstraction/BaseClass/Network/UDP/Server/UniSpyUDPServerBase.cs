using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NetCoreServer;
using Serilog.Events;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass.Network.UDP;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Network
{
    /// <summary>
    /// This is a template class that helps creating a UDP Server with
    /// logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public abstract class UniSpyUDPServerBase : UdpServer, IUniSpyServer
    {
        public Guid ServerID { get; private set; }
        /// <summary>
        /// currently, we do not to care how to delete elements in dictionary
        /// </summary>
        public UniSpyUDPSessionManagerBase SessionManager { get; protected set; }
        UniSpySessionManagerBase IUniSpyServer.SessionManager => SessionManager;
        private Func<byte[], string> _encodingMessageGetString => Encoding.ASCII.GetString;
        private Func<string, byte[]> _encodingMessageGetBytes => Encoding.ASCII.GetBytes;
        public UniSpyUDPServerBase(Guid serverID, IPEndPoint endpoint) : base(endpoint)
        {
            ServerID = serverID;
        }
        public override bool Start()
        {
            SessionManager.Start();
            return base.Start();
        }

        protected override void OnStarted() => ReceiveAsync();
        protected override void OnError(SocketError error) => LogWriter.ToLog(LogEventLevel.Error, error.ToString());
        protected virtual UniSpyUDPSessionBase CreateSession(EndPoint endPoint) => new UniSpyUDPSessionBase(this, endPoint);
        #region Raw message sending method
        public bool BaseSendAsync(EndPoint endPoint, string buffer) => BaseSendAsync(endPoint, _encodingMessageGetBytes(buffer));
        public bool BaseSendAsync(EndPoint endPoint, byte[] buffer) => BaseSendAsync(endPoint, buffer, 0, buffer.Length);
        public bool BaseSendAsync(EndPoint endpoint, byte[] buffer, long offset, long size) => base.SendAsync(endpoint, buffer, offset, size);
        #endregion
        /// <summary>
        /// Continue receive datagrams
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="sent"></param>
        protected override void OnSent(EndPoint endpoint, long sent) => ReceiveAsync();


        protected virtual void OnReceived(UniSpyUDPSessionBase session, string message) { }
        protected virtual void OnReceived(UniSpyUDPSessionBase session, byte[] message) => OnReceived(session, _encodingMessageGetString(message));
        protected override void OnReceived(EndPoint endPoint, byte[] buffer, long offset, long size)
        {
            //even if we did not response we keep receive message
            ReceiveAsync();
            UniSpyUDPSessionBase tempSession;
            if (SessionManager.SessionPool.ContainsKey((IPEndPoint)endPoint))
            {
                IUniSpySession result;
                SessionManager.SessionPool.TryGetValue((IPEndPoint)endPoint, out result);
                tempSession = (UniSpyUDPSessionBase)result;
                tempSession.LastPacketReceivedTime = DateTime.Now;
            }
            else
            {
                tempSession = CreateSession(endPoint);
                SessionManager.SessionPool.TryAdd(tempSession.RemoteIPEndPoint, tempSession);
            }

            OnReceived(tempSession, buffer.Take((int)size).ToArray());
        }
    }
}
