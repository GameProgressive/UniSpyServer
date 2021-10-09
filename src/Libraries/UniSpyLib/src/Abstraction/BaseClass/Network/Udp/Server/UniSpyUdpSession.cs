using System;
using System.Net;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Encryption;

namespace UniSpyLib.Abstraction.BaseClass.Network.Udp.Server
{
    /// <summary>
    /// A remote endpoint wrapper for UDP server which unifies the interface for <see cref="IUniSpySession"/>
    /// </summary>
    public class UniSpyUdpSession : IUniSpySession
    {
        public UniSpyUdpServer Server { get; private set; }
        public EndPoint RemoteEndPoint { get; private set; }
        public IPEndPoint RemoteIPEndPoint => (IPEndPoint)RemoteEndPoint;
        public DateTime LastPacketReceivedTime { get; protected set; }
        public TimeSpan SessionExistedTime => DateTime.Now.Subtract(LastPacketReceivedTime);

        public UniSpyUdpSession(UniSpyUdpServer server, EndPoint endPoint)
        {
            Server = server;
            RemoteEndPoint = endPoint;
            LastPacketReceivedTime = DateTime.Now;
        }
        public virtual void OnReceived(string message) { }
        public virtual void OnReceived(byte[] message) => OnReceived(UniSpyEncoding.GetString(message));
        public bool Send(IUniSpyResponse response) => Server.Send(RemoteEndPoint, response);
        public bool BaseSend(IUniSpyResponse response) => Server.BaseSend(RemoteEndPoint, response);

    }
}
