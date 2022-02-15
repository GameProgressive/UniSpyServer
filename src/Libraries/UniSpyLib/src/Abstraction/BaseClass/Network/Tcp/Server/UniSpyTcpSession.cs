using System;
using System.Net;
using NetCoreServer;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Events;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server
{
    /// <summary>
    /// This is a template class that helps creating a TCP Session (formerly TCP stream)
    /// with logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public class UniSpyTcpSession : TcpSession, ISession
    {
        public EndPoint RemoteEndPoint { get; private set; }
        public IPEndPoint RemoteIPEndPoint => (IPEndPoint)RemoteEndPoint;
        public new UniSpyTcpServer Server => (UniSpyTcpServer)base.Server;
        public event OnConnectedEventHandler OnConnect;
        public event OnDisconnectedEventHandler OnDisconnect;
        public event OnReceivedEventHandler OnReceive;
        public UniSpyTcpSession(UniSpyTcpServer server) : base(server)
        {
        }
        protected override void OnConnected()
        {
            OnConnect();
            base.OnConnected();
        }
        protected override void OnDisconnected()
        {
            OnDisconnect();
            base.OnDisconnected();
        }
        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            OnReceive(buffer);
            base.OnReceived(buffer, offset, size);
        }

        public bool Send(IResponse response)
        {
            throw new NotImplementedException();
        }
        // /// <summary>
        // /// Send unencrypted data
        // /// </summary>
        // /// <param name="buffer">plaintext</param>
        // /// <returns>is sending succeed</returns>
        // public bool BaseSendAsync(string buffer) => BaseSendAsync(UniSpyEncoding.GetBytes(buffer));
        // /// <summary>
        // /// Send unencrypted data
        // /// </summary>
        // /// <param name="buffer">plaintext</param>
        // /// <returns>is sending succeed</returns>
        // public bool BaseSendAsync(byte[] buffer)
        // {
        //     LogWriter.LogNetworkSending(RemoteIPEndPoint, buffer);
        //     return base.SendAsync(buffer, 0, buffer.Length);
        // }
        // public override bool SendAsync(string buffer) => base.SendAsync(buffer);
        // public override bool SendAsync(byte[] buffer, long offset, long size)
        // {
        //     byte[] plainText = buffer.Skip((int)offset).Take((int)size).ToArray();
        //     LogWriter.LogNetworkSending(RemoteIPEndPoint, plainText);
        //     byte[] cipherText = Encrypt(plainText);
        //     Array.Copy(cipherText, buffer, size);
        //     return base.SendAsync(buffer, offset, size);
        // }
        // protected virtual void OnReceived(string message) { }
        // protected virtual void OnReceived(byte[] buffer) => OnReceived(UniSpyEncoding.GetString(buffer));
        // protected override void OnReceived(byte[] buffer, long offset, long size)
        // {
        //     if (RemoteEndPoint is null)
        //     {
        //         RemoteEndPoint = Socket.RemoteEndPoint;
        //     }
        //     byte[] cipherText = buffer.Skip((int)offset).Take((int)size).ToArray();
        //     byte[] plainText = Decrypt(cipherText);
        //     LogWriter.LogNetworkReceiving(RemoteIPEndPoint, plainText);
        //     OnReceived(plainText);
        // }

        // /// <summary>
        // /// The virtual method override by child class, which helps child class to encrypt data
        // /// </summary>
        // /// <param name="buffer">plaintext</param>
        // /// <returns>ciphertext</returns>
        // protected virtual byte[] Encrypt(byte[] buffer) => buffer;
        // /// <summary>
        // /// The virtual method override by child class, which helps child class to decrypt data
        // /// </summary>
        // /// <param name="buffer">ciphertext</param>
        // /// <returns>plaintext</returns>
        // protected virtual byte[] Decrypt(byte[] buffer) => buffer;

        // public bool Send(IResponse response)
        // {
        //     response.Build();
        //     if (response.SendingBuffer == null)
        //     {
        //         throw new UniSpyException("SendingBuffer can not be null");
        //     }
        //     var bufferType = response.SendingBuffer.GetType();

        //     if (bufferType == typeof(string))
        //     {
        //         return SendAsync((string)response.SendingBuffer);
        //     }
        //     else if (bufferType == typeof(byte[]))
        //     {
        //         return SendAsync((byte[])response.SendingBuffer);
        //     }
        //     else
        //     {
        //         throw new UniSpyException("The buffer type is invalid");
        //     }
        // }

        // public bool BaseSend(IResponse response)
        // {
        //     response.Build();
        //     if (response.SendingBuffer == null)
        //     {
        //         throw new UniSpyException("SendingBuffer can not be null");
        //     }
        //     var bufferType = response.SendingBuffer.GetType();

        //     if (bufferType == typeof(string))
        //     {
        //         return BaseSendAsync((string)response.SendingBuffer);
        //     }
        //     else if (bufferType == typeof(byte[]))
        //     {
        //         return BaseSendAsync((byte[])response.SendingBuffer);
        //     }
        //     else
        //     {
        //         throw new UniSpyException("The buffer type is invalid");
        //     }
        // }
    }
}

