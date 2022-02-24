using System;
using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Encryption;
using UniSpyServer.UniSpyLib.Events;

namespace UniSpyServer.UniSpyLib.Application.Network.Udp.Server
{
    /// <summary>
    /// A remote endpoint wrapper for UDP server which unifies the interface for <see cref="ISession"/>
    /// </summary>
    public class UdpSession : IUdpSession
    {
        public UdpServer Server { get; private set; }
        public IPEndPoint RemoteIPEndPoint { get; private set; }
        public DateTime LastPacketReceivedTime { get; protected set; }
        public TimeSpan SessionExistedTime => DateTime.Now.Subtract(LastPacketReceivedTime);
        IServer ISession.Server => Server;
        public event OnReceivedEventHandler OnReceive;
        public UdpSession(UdpServer server, EndPoint endPoint)
        {
            Server = server;
            RemoteIPEndPoint = (IPEndPoint)endPoint;
            LastPacketReceivedTime = DateTime.Now;
        }
        public virtual void OnReceived(byte[] message)
        {
            OnReceive(message);
        }

        public bool Send(object response)
        {
            if (response.GetType() == typeof(string))
            {
                return Server.SendAsync(RemoteIPEndPoint, UniSpyEncoding.GetBytes((string)response));
            }
            else if (response.GetType() == typeof(byte[]))
            {
                return Server.SendAsync(RemoteIPEndPoint, (byte[])response);
            }
            else
            {
                throw new UniSpyException("UniSpyTcpSession.Send: response must be string or byte[]");
            }
        }
        public void Send(string response)
        {
            Send(RemoteIPEndPoint, UniSpyEncoding.GetBytes(response));
        }
        public void Send(byte[] response)
        {
            Server.SendAsync(RemoteIPEndPoint, response);
        }
        public void Send(IPEndPoint endPoint, string response)
        {
            Send(endPoint, UniSpyEncoding.GetBytes(response));
        }
        public void Send(IPEndPoint endPoint, byte[] response)
        {
            Server.SendAsync(endPoint, response);
        }
    }
}
