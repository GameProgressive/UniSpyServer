using System.Net;
using System.Threading;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.Events;

namespace UniSpy.Server.Core.Abstraction.BaseClass.Network.Udp.Server
{
    /// <summary>
    /// A remote endpoint wrapper for UDP server which unifies the interface for <see cref="IConnection"/>
    /// </summary>
    public class UdpConnection : IUdpConnection
    {
        public UdpServer Server { get; private set; }
        public IPEndPoint RemoteIPEndPoint { get; private set; }
        IServer IConnection.Server => Server;
        public NetworkConnectionType ConnectionType => NetworkConnectionType.Udp;
        public event OnReceivedEventHandler OnReceive;

        public UdpConnection(UdpServer server, IPEndPoint endPoint)
        {
            Server = server;
            RemoteIPEndPoint = endPoint;
        }
        public virtual void OnReceived(byte[] message)
        {
            // Server.ReceiveAsync();
            ThreadPool.QueueUserWorkItem(o => { try { Server.ReceiveAsync(); } catch { } });
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
                throw new UniSpyException("IUdpConnection.Send: response must be string or byte[]");
            }
        }
        public void Send(string response) => Send(RemoteIPEndPoint, UniSpyEncoding.GetBytes(response));
        public void Send(byte[] response) => Server.Send(RemoteIPEndPoint, response);
        public void Send(IPEndPoint endPoint, string response) => Send(endPoint, UniSpyEncoding.GetBytes(response));
        public void Send(IPEndPoint endPoint, byte[] response) => Server.Send(endPoint, response);
    }
}
