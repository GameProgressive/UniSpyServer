using NetCoreServer;
using Serilog.Events;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Encryption;
using UniSpyLib.Logging;

namespace UniSpyLib.Network
{
    /// <summary>
    /// This is a template class that helps creating a TCP Session (formerly TCP stream)
    /// with logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public class UniSpyTCPSessionBase : TcpSession, IUniSpySession
    {
        public EndPoint RemoteEndPoint => Socket.RemoteEndPoint;
        public IPEndPoint RemoteIPEndPoint
        {
            get
            {
                try
                {
                    return (IPEndPoint)Socket.RemoteEndPoint;
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Client already disconnected.", e);
                }
            }
        }
        public new UniSpyTCPServerBase Server => (UniSpyTCPServerBase)base.Server;
        public UniSpyTCPSessionBase(UniSpyTCPServerBase server) : base(server)
        {
        }
        /// <summary>
        /// Send unencrypted data
        /// </summary>
        /// <param name="buffer">plaintext</param>
        /// <returns>is sending succeed</returns>
        public bool BaseSendAsync(string buffer) => BaseSendAsync(UniSpyEncoding.GetBytes(buffer));
        /// <summary>
        /// Send unencrypted data
        /// </summary>
        /// <param name="buffer">plaintext</param>
        /// <returns>is sending succeed</returns>
        public bool BaseSendAsync(byte[] buffer)
        {
            LogWriter.LogNetworkSending(RemoteIPEndPoint, buffer);
            return base.SendAsync(buffer, 0, buffer.Length);
        }
        public override bool SendAsync(string buffer) => base.SendAsync(buffer);
        public override bool SendAsync(byte[] buffer, long offset, long size)
        {
            byte[] plainText = buffer.Skip((int)offset).Take((int)size).ToArray();
            LogWriter.LogNetworkSending(RemoteIPEndPoint, plainText);
            byte[] cipherText = Encrypt(plainText);
            Array.Copy(cipherText, buffer, size);
            return base.SendAsync(buffer, offset, size);
        }
        protected virtual void OnReceived(string message) { }
        protected virtual void OnReceived(byte[] buffer) => OnReceived(UniSpyEncoding.GetString(buffer));
        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            byte[] cipherText = buffer.Skip((int)offset).Take((int)size).ToArray();
            byte[] plainText = Decrypt(cipherText);
            LogWriter.LogNetworkReceiving(RemoteIPEndPoint, plainText);
            OnReceived(plainText);
        }

        /// <summary>
        /// The virtual method override by child class, which helps child class to encrypt data
        /// </summary>
        /// <param name="buffer">plaintext</param>
        /// <returns>ciphertext</returns>
        protected virtual byte[] Encrypt(byte[] buffer) => buffer;
        /// <summary>
        /// The virtual method override by child class, which helps child class to decrypt data
        /// </summary>
        /// <param name="buffer">ciphertext</param>
        /// <returns>plaintext</returns>
        protected virtual byte[] Decrypt(byte[] buffer) => buffer;
    }
}

