using System.Net;

namespace UniSpyLib.Abstraction.Interface
{
    public interface IUniSpySession
    {
        EndPoint RemoteEndPoint { get; }
        IPEndPoint RemoteIPEndPoint { get; }
        bool SendAsync(object buffer);
        bool BaseSendAsync(object buffer);
    }
}
