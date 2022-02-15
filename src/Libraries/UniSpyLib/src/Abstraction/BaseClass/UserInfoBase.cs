using System.Net;
using UniSpyServer.UniSpyLib.Events;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass
{
    public abstract class UserInfoBase
    {
        public IPEndPoint RemoteIPEndPoint { get; private set; }
        public UserInfoBase(IPEndPoint remoteIPEndPoint)
        {
            RemoteIPEndPoint = remoteIPEndPoint;
        }
    }
}