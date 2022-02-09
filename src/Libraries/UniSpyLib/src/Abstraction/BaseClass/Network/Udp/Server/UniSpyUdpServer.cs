using System;
using System.Linq;
using System.Net;
using NetCoreServer;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Encryption;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Udp.Server
{
    /// <summary>
    /// This is a template class that helps creating a UDP Server with
    /// logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public abstract class UniSpyUdpServer : UdpServer, IServer
    {
        public Guid ServerID { get; private set; }
        /// <summary>
        /// currently, we do not to care how to delete elements in dictionary
        /// </summary>
        public UniSpyUdpSessionManager SessionManager { get; protected set; }
        SessionManager IServer.SessionManager => SessionManager;
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


        /// <summary>
        /// Send unencrypted data
        /// </summary>
        /// <param name="buffer">plaintext</param>
        /// <returns>is sending succeed</returns>
        protected override void OnReceived(EndPoint endPoint, byte[] buffer, long offset, long size)
        {
            //even if we did not response we keep receive message
            UniSpyUdpSession session;
            if (SessionManager.SessionPool.ContainsKey((IPEndPoint)endPoint))
            {
                session = (UniSpyUdpSession)SessionManager.SessionPool[(IPEndPoint)endPoint];
            }
            else
            {
                session = CreateSession(endPoint);
                SessionManager.SessionPool.Add(session.RemoteIPEndPoint, session);
            }
            byte[] cipherText = buffer.Skip((int)offset).Take((int)size).ToArray();
            byte[] plainText = Decrypt(cipherText);
            LogWriter.LogNetworkReceiving(session.RemoteIPEndPoint, plainText);
            // WAINING!!!!!!: Do not change the sequence of ReceiveAsync()
            ReceiveAsync();
            session.OnReceived(plainText);
        }
        protected virtual void OnReceived(UniSpyUdpSession session, byte[] message) { }
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


        public bool Send(EndPoint endpoint, IResponse response)
        {
            response.Build();
            if (response.SendingBuffer == null)
            {
                throw new UniSpyException("SendingBuffer can not be null");
            }
            var bufferType = response.SendingBuffer.GetType();
            if (bufferType == typeof(string))
            {
                return SendAsync(endpoint, UniSpyEncoding.GetBytes((string)response.SendingBuffer));
            }
            else if (bufferType == typeof(byte[]))
            {
                return SendAsync(endpoint, (byte[])response.SendingBuffer);
            }
            else
            {
                throw new UniSpyException("The buffer type is invalid");
            }
        }

        public bool BaseSend(EndPoint endpoint, IResponse response)
        {
            response.Build();
            if (response.SendingBuffer == null)
            {
                throw new UniSpyException("SendingBuffer can not be null");
            }
            var bufferType = response.SendingBuffer.GetType();
            if (bufferType == typeof(string))
            {
                return BaseSendAsync(endpoint, UniSpyEncoding.GetBytes((string)response.SendingBuffer));
            }
            else if (bufferType == typeof(byte[]))
            {
                return BaseSendAsync(endpoint, (byte[])response.SendingBuffer);
            }
            else
            {
                throw new UniSpyException("The buffer type is invalid");
            }
        }
    }
}
