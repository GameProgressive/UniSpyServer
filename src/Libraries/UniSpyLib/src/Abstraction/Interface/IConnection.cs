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
        bool Send(object response);
        // void Send(string response);
        // void Send(byte[] response);
    }
    /// <summary>
    /// Represent a udp connection
    /// </summary>
    public interface IUdpSession : ISession
    {
        bool Send(IPEndPoint endPoint, object response);
    }
    /// <summary>
    /// Represent a tcp connection
    /// </summary>
    public interface ITcpSession : ISession
    {
        void Disconnect();
    }
}
