using System;
using System.Net;
using UniSpyServer.UniSpyLib.Events;

namespace UniSpyServer.UniSpyLib.Abstraction.Interface
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
    public interface ISession
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
    public interface IUdpSession : ISession
    {
        public TimeSpan SessionExistedTime { get; }
        void Send(IPEndPoint endPoint, byte[] response);
        void Send(IPEndPoint endPoint, string response);
    }
    /// <summary>
    /// Represent a tcp connection
    /// </summary>
    public interface ITcpSession : ISession
    {
        event OnConnectedEventHandler OnConnect;
        event OnDisconnectedEventHandler OnDisconnect;
        void Disconnect();
    }
    public interface IHttpSession : ITcpSession
    {
    }
}
