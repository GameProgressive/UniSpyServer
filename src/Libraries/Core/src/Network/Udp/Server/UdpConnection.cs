using System.Net;
using System.Threading;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.Events;

namespace UniSpy.Server.Core.Network.Udp.Server
{
    /// <summary>
    /// A remote endpoint wrapper for UDP server which unifies the interface for <see cref="IConnection"/>
    /// </summary>
    public class UdpConnection : IUdpConnection
    {
        public UdpConnectionManager Manager { get; private set; }
        public IPEndPoint RemoteIPEndPoint { get; private set; }
        IConnectionManager IConnection.Manager => Manager;
        public NetworkConnectionType ConnectionType => NetworkConnectionType.Udp;
        public event OnReceivedEventHandler OnReceive;

        public UdpConnection(UdpConnectionManager server, IPEndPoint endPoint)
        {
            Manager = server;
            RemoteIPEndPoint = endPoint;
        }
        public virtual void OnReceived(byte[] message)
        {
            // Server.ReceiveAsync();
            ThreadPool.QueueUserWorkItem(o => { try { Manager.ReceiveAsync(); } catch { } });
            OnReceive(message);
        }

        public bool Send(object response)
        {
            if (response.GetType() == typeof(string))
            {
                return Manager.SendAsync(RemoteIPEndPoint, UniSpyEncoding.GetBytes((string)response));
            }
            else if (response.GetType() == typeof(byte[]))
            {
                return Manager.SendAsync(RemoteIPEndPoint, (byte[])response);
            }
            else
            {
                throw new UniSpy.Exception("IUdpConnection.Send: response must be string or byte[]");
            }
        }
        public void Send(string response) => Send(RemoteIPEndPoint, UniSpyEncoding.GetBytes(response));
        public void Send(byte[] response) => Manager.Send(RemoteIPEndPoint, response);
        public void Send(IPEndPoint endPoint, string response) => Send(endPoint, UniSpyEncoding.GetBytes(response));
        public void Send(IPEndPoint endPoint, byte[] response) => Manager.Send(endPoint, response);
    }
}
