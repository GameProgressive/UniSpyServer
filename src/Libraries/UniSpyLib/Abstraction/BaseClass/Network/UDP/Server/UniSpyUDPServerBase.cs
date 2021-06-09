using NetCoreServer;
using Serilog.Events;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass.Network.UDP;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Encryption;
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
            if (OptionSendBufferSize > int.MaxValue || OptionReceiveBufferSize > int.MaxValue)
            {
                throw new ArgumentException("Buffer size can not big than length of integer!");
            }
            SessionManager.Start();
            return base.Start();
        }

        protected override void OnStarted() => ReceiveAsync();
        protected override void OnError(SocketError error) => LogWriter.ToLog(LogEventLevel.Error, error.ToString());
        protected virtual UniSpyUDPSessionBase CreateSession(EndPoint endPoint) => new UniSpyUDPSessionBase(this, endPoint);
        /// <summary>
        /// Continue receive datagrams
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="sent"></param>
        protected override void OnSent(EndPoint endpoint, long sent) => ReceiveAsync();
        protected virtual void OnReceived(UniSpyUDPSessionBase session, string message) { }
        protected virtual void OnReceived(UniSpyUDPSessionBase session, byte[] message) => OnReceived(session, UniSpyEncoding.GetString(message));

        /// <summary>
        /// Send unencrypted data
        /// </summary>
        /// <param name="buffer">plaintext</param>
        /// <returns>is sending succeed</returns>
        protected override void OnReceived(EndPoint endPoint, byte[] buffer, long offset, long size)
        {
            //even if we did not response we keep receive message
            ReceiveAsync();
            UniSpyUDPSessionBase session;
            if (SessionManager.SessionPool.ContainsKey((IPEndPoint)endPoint))
            {
                IUniSpySession result;
                SessionManager.SessionPool.TryGetValue((IPEndPoint)endPoint, out result);
                session = (UniSpyUDPSessionBase)result;
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
