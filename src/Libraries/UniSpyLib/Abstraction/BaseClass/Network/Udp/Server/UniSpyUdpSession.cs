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

        public bool SendAsync(object buffer)
        {
            if (buffer.GetType() == typeof(string))
            {
                return Server.SendAsync(RemoteEndPoint, UniSpyEncoding.GetBytes((string)buffer));
            }
            else if (buffer.GetType() == typeof(byte[]))
            {
                return Server.SendAsync(RemoteEndPoint, (byte[])buffer);
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
                return Server.BaseSendAsync(RemoteEndPoint, UniSpyEncoding.GetBytes((string)buffer));
            }
            else if (buffer.GetType() == typeof(byte[]))
            {
                return Server.BaseSendAsync(RemoteEndPoint, (byte[])buffer);
            }
            else
            {
                throw new UniSpyException("The buffer type is invalid");
            }
        }
    }
}
