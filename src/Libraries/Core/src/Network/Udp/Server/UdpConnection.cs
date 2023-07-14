using System.Threading.Tasks;
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
            Task.Run(() => OnReceive(message));
        }

        public bool Send(object response)
        {
            if (response.GetType() == typeof(string))
            {
                Manager.Send(RemoteIPEndPoint, UniSpyEncoding.GetBytes((string)response));
                return true;
            }
            else if (response.GetType() == typeof(byte[]))
            {
                Manager.Send(RemoteIPEndPoint, (byte[])response);
                return true;
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
