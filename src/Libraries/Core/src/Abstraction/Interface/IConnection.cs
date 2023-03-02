using System.Net;
using UniSpy.Server.Core.Events;

namespace UniSpy.Server.Core.Abstraction.Interface
{

    public enum NetworkConnectionType
    {
        Tcp,
        Udp,
        Http,
        Https,
        Test,
    }
    /// <summary>
    /// Represent a remote connection, tcp, udp, http etc.
    /// </summary>
    public interface IConnection
    {
        event OnReceivedEventHandler OnReceive;
        IServer Server { get; }
        IPEndPoint RemoteIPEndPoint { get; }
        void Send(string response);
        void Send(byte[] response);
        NetworkConnectionType ConnectionType { get; }
    }
    /// <summary>
    /// Represent a udp connection
    /// </summary>
    public interface IUdpConnection : IConnection
    {
        // public TimeSpan ConnectionExistedTime { get; }
        void Send(IPEndPoint endPoint, byte[] response);
        void Send(IPEndPoint endPoint, string response);
    }
    /// <summary>
    /// Represent a tcp connection
    /// </summary>
    public interface ITcpConnection : IConnection
    {
        event OnConnectedEventHandler OnConnect;
        event OnDisconnectedEventHandler OnDisconnect;
        void Disconnect();
    }
    public interface IHttpConnection : ITcpConnection
    {
    }
}
