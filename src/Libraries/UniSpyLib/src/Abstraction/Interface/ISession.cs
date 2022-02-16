using System.Net;
using UniSpyServer.UniSpyLib.Events;

namespace UniSpyServer.UniSpyLib.Abstraction.Interface
{
    public interface ISession
    {
        IServer Server { get; }
        EndPoint RemoteEndPoint { get; }
        IPEndPoint RemoteIPEndPoint { get; }
        bool Send(object response);
        // void Send(string response);
        // void Send(byte[] response);
    }
    public interface IUdpSession : ISession
    {
        bool Send(IPEndPoint endPoint, object response);
    }
    public interface ITcpSession : ISession
    {

    }
}
