using NetCoreServer;
using System;
using System.Linq;
using System.Net;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Encryption;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass.Network.Udp.Server
{
    /// <summary>
    /// This is a template class that helps creating a UDP Server with
    /// logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public abstract class UniSpyUdpServer : UdpServer, IUniSpyServer
    {
        public Guid ServerID { get; private set; }
        /// <summary>
        /// currently, we do not to care how to delete elements in dictionary
        /// </summary>
        public UniSpyUdpSessionManager SessionManager { get; protected set; }
        UniSpySessionManager IUniSpyServer.SessionManager => SessionManager;
        public UniSpyUdpServer(Guid serverID, IPEndPoint endpoint) : base(endpoint)
        {
            ServerID = serverID;
        }
        public override bool Start()
        {
            if (OptionSendBufferSize > int.MaxValue || OptionReceiveBufferSize > int.MaxValue)
            {
                throw new ArgumentException("Buffer size can not big than length of integer!");
            }
            SessionManager.Start();
            return base.Start();
        }

        protected override void OnStarted() => ReceiveAsync();
        protected virtual UniSpyUdpSession CreateSession(EndPoint endPoint) => new UniSpyUdpSession(this, endPoint);
        /// <summary>
        /// Continue receive datagrams
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="sent"></param>
        protected override void OnSent(EndPoint endpoint, long sent) => ReceiveAsync();
        protected virtual void OnReceived(UniSpyUdpSession session, string message) { }
        protected virtual void OnReceived(UniSpyUdpSession session, byte[] message) => OnReceived(session, UniSpyEncoding.GetString(message));

        /// <summary>
        /// Send unencrypted data
        /// </summary>
        /// <param name="buffer">plaintext</param>
        /// <returns>is sending succeed</returns>
        protected override void OnReceived(EndPoint endPoint, byte[] buffer, long offset, long size)
        {
            //even if we did not response we keep receive message
            ReceiveAsync();
            UniSpyUdpSession session;
            if (SessionManager.SessionPool.ContainsKey((IPEndPoint)endPoint))
            {
                IUniSpySession result;
                SessionManager.SessionPool.TryGetValue((IPEndPoint)endPoint, out result);
                session = (UniSpyUdpSession)result;
                session.LastPacketReceivedTime = DateTime.Now;
            }
            else
            {
                session = CreateSession(endPoint);
                SessionManager.SessionPool.TryAdd(session.RemoteIPEndPoint, session);
            }
            byte[] cipherText = buffer.Skip((int)offset).Take((int)size).ToArray();
            byte[] plainText = Decrypt(cipherText);
            LogWriter.LogNetworkReceiving(session.RemoteIPEndPoint, plainText);
            OnReceived(session, plainText);
        }
        public bool BaseSendAsync(EndPoint endPoint, string buffer) => BaseSendAsync(endPoint, UniSpyEncoding.GetBytes(buffer));
        /// <summary>
        /// Send unencrypted data
        /// </summary>
        /// <param name="buffer">plaintext</param>
        /// <returns>is sending succeed</returns>
        public bool BaseSendAsync(EndPoint endPoint, byte[] buffer)
        {
            LogWriter.LogNetworkSending((IPEndPoint)endPoint, buffer);
            return base.SendAsync(endPoint, buffer, 0, buffer.Length);
        }
        public override bool SendAsync(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            byte[] plainText = buffer.Skip((int)offset).Take((int)size).ToArray();
            LogWriter.LogNetworkSending((IPEndPoint)endpoint, plainText);
            byte[] cipherText = Encrypt(plainText);
            Array.Copy(cipherText, buffer, size);
            return base.SendAsync(endpoint, buffer, offset, size);
        }
        /// <summary>
        /// The virtual method, which helps child class to encrypt data
        /// </summary>
        /// <param name="buffer">plaintext</param>
        /// <returns>ciphertext</returns>
        protected virtual byte[] Encrypt(byte[] buffer) => buffer;
        /// <summary>
        /// The virtual method, which helps child class to decrypt data
        /// </summary>
        /// <param name="buffer">ciphertext</param>
        /// <returns>plaintext</returns>
        protected virtual byte[] Decrypt(byte[] buffer) => buffer;
    }
}
