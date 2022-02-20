using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Encryption;
using UniSpyServer.UniSpyLib.Events;

namespace UniSpyServer.UniSpyLib.Application.Network.Tcp.Server
{
    /// <summary>
    /// This is a template class that helps creating a TCP Session (formerly TCP stream)
    /// with logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public class TcpSession : NetCoreServer.TcpSession, ITcpSession
    {
        public IPEndPoint RemoteIPEndPoint { get; private set; }
        public new TcpServer Server => (TcpServer)base.Server;
        IServer ISession.Server => Server;
        public event OnConnectedEventHandler OnConnect;
        public event OnDisconnectedEventHandler OnDisconnect;
        public event OnReceivedEventHandler OnReceive;
        public TcpSession(TcpServer server) : base(server)
        {
            RemoteIPEndPoint = (IPEndPoint)Socket.RemoteEndPoint;
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

        public bool Send(object response)
        {
            if (response.GetType() == typeof(string))
            {
                return SendAsync(UniSpyEncoding.GetBytes((string)response));
            }
            else if (response.GetType() == typeof(byte[]))
            {
                return SendAsync((byte[])response);
            }
            else
            {
                throw new UniSpyException("UniSpyTcpSession.Send: response must be string or byte[]");
            }
        }

        void ITcpSession.Disconnect() => Disconnect();

        void ISession.Send(string response)
        {
            SendAsync(response);
        }

        void ISession.Send(byte[] response)
        {
            SendAsync(response);
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

