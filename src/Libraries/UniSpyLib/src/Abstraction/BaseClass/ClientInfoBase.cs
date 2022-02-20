using System.Net;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass
{
    public abstract class ClientInfoBase
    {
        public IPEndPoint RemoteIPEndPoint { get; private set; }
        public ClientInfoBase(IPEndPoint remoteIPEndPoint)
        {
            RemoteIPEndPoint = remoteIPEndPoint;
        }
    }
}