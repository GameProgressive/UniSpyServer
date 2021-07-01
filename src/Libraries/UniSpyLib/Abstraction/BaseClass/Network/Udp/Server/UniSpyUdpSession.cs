using System;
using System.Net;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Encryption;

namespace UniSpyLib.Abstraction.BaseClass.Network.Udp.Server
{
    public class UniSpyUdpSession : IUniSpySession
    {
        public UniSpyUdpServer Server { get; private set; }
        public EndPoint RemoteEndPoint { get; private set; }
        public IPEndPoint RemoteIPEndPoint => (IPEndPoint)RemoteEndPoint;
        public DateTime LastPacketReceivedTime { get; set; }
        public TimeSpan SessionExistedTime => DateTime.Now.Subtract(LastPacketReceivedTime);
        public UniSpyUdpSession(UniSpyUdpServer server, EndPoint endPoint)
        {
            Server = server;
            RemoteEndPoint = endPoint;
            LastPacketReceivedTime = DateTime.Now;
        }

        public bool SendAsync(byte[] buffer, long offset, long size) => Server.SendAsync(RemoteEndPoint, buffer, offset, size);
        public bool SendAsync(string text) => Server.SendAsync(RemoteEndPoint, UniSpyEncoding.GetBytes(text));
        public bool SendAsync(byte[] buffer) => Server.SendAsync(RemoteEndPoint, buffer);
        public bool BaseSendAsync(byte[] buffer) => Server.BaseSendAsync(RemoteEndPoint, buffer);
        public bool BaseSendAsync(string buffer) => Server.BaseSendAsync(RemoteEndPoint, buffer);

        public bool SendAsync(object buffer)
        {
            if (buffer.GetType() == typeof(string))
            {
                return SendAsync((string)buffer);
            }
            else if (buffer.GetType() == typeof(byte[]))
            {
                return SendAsync((byte[])buffer);
            }
            else
            {
                throw new UniSpyException("The buffer type is invalid");
            }
        }

        public bool BaseSendAsync(object buffer)
        {
            if (buffer.GetType() == typeof(string))
            {
                return BaseSendAsync((string)buffer);
            }
            else if (buffer.GetType() == typeof(byte[]))
            {
                return BaseSendAsync((byte[])buffer);
            }
            else
            {
                throw new UniSpyException("The buffer type is invalid");
            }
        }
    }
}
