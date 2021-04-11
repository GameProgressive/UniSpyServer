using System.Net;

namespace UniSpyLib.Abstraction.Interface
{
    public interface IUniSpySession
    {
        EndPoint RemoteEndPoint { get; }
        IPEndPoint RemoteIPEndPoint { get; }
        long Send(string text);
        long Send(byte[] buffer);
        bool SendAsync(string text);
        bool SendAsync(byte[] buffer);
        bool BaseSendAsync(string buffer);
        bool BaseSendAsync(byte[] buffer);
    }
}
