using System.Net;

namespace UniSpyServer.UniSpyLib.Abstraction.Interface
{
    /// <summary>
    /// Represent a remote connection, tcp, udp, http etc.
    /// </summary>
    public interface ISession
    {
        IServer Server { get; }
        IPEndPoint RemoteIPEndPoint { get; }
        void Send(string response);
        void Send(byte[] response);
    }
    /// <summary>
    /// Represent a udp connection
    /// </summary>
    public interface IUdpSession : ISession
    {
        void Send(IPEndPoint endPoint, byte[] response);
        void Send(IPEndPoint endPoint, string response);
    }
    /// <summary>
    /// Represent a tcp connection
    /// </summary>
    public interface ITcpSession : ISession
    {
        void Disconnect();
    }
    public interface IHttpSession:ITcpSession
    {
        
    }
}
