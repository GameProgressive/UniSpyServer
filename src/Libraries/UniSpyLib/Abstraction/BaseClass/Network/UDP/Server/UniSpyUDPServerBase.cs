using System;
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

        public UniSpyUDPServerBase(Guid serverID, IPEndPoint endpoint) : base(endpoint)
        {
            ServerID = serverID;
        }
        public override bool Start()
        {
            SessionManager.Start();
            return base.Start();
        }
        protected virtual UniSpyUDPSessionBase CreateSession(EndPoint endPoint) => new UniSpyUDPSessionBase(this, endPoint);


        protected override void OnStarted() => ReceiveAsync();
        protected override void OnError(SocketError error) => LogWriter.ToLog(LogEventLevel.Error, error.ToString());

        public bool BaseSendAsync(EndPoint endPoint, string buffer) => BaseSendAsync(endPoint, Encoding.ASCII.GetBytes(buffer));
        public bool BaseSendAsync(EndPoint endPoint, byte[] buffer) => BaseSendAsync(endPoint, buffer, 0, buffer.Length);
        public bool BaseSendAsync(EndPoint endpoint, byte[] buffer, long offset, long size) => base.SendAsync(endpoint, buffer, offset, size);

        public override long Send(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            LogWriter.LogNetworkTraffic("Send", Endpoint, buffer, size);
            return base.Send(endpoint, buffer, offset, size);
        }

        public override bool SendAsync(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            LogWriter.LogNetworkTraffic("Send", Endpoint, buffer, size);
            return base.SendAsync(endpoint, buffer, offset, size);
        }


        /// <summary>
        /// Continue receive datagrams
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="sent"></param>
        protected override void OnSent(EndPoint endpoint, long sent) => ReceiveAsync();


        protected virtual void OnReceived(UniSpyUDPSessionBase session, string message) { }
        protected virtual void OnReceived(UniSpyUDPSessionBase session, byte[] message) => OnReceived(session, Encoding.ASCII.GetString(message));
        protected override void OnReceived(EndPoint endPoint, byte[] buffer, long offset, long size)
        {
            // Need at least 2 bytes
            if (size < 2 || size > OptionReceiveBufferSize)
            {
                LogWriter.LogNetworkSpam((IPEndPoint)endPoint);
                return;
            }
            byte[] message = new byte[(int)size];
            Array.Copy(buffer, 0, message, 0, (int)size);

            LogWriter.LogNetworkTraffic("Recv", (IPEndPoint)endPoint, buffer, size);
            //even if we did not response we keep receive message
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

            ReceiveAsync();
            OnReceived(tempSession, message);
        }

        #region Encryption
        protected virtual byte[] Encrypt(byte[] buffer) => buffer;
        protected virtual byte[] Decryption(byte[] buffer) => buffer;
        #endregion
    }
}
