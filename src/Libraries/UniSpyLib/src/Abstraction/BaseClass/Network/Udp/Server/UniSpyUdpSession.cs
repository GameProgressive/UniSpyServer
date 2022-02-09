using System;
using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Encryption;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Udp.Server
{
    /// <summary>
    /// A remote endpoint wrapper for UDP server which unifies the interface for <see cref="ISession"/>
    /// </summary>
    public class UniSpyUdpSession : ISession
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
        public bool Send(IResponse response) => Server.Send(RemoteEndPoint, response);
        public bool BaseSend(IResponse response) => Server.BaseSend(RemoteEndPoint, response);

    }
}
