using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.CDKey.Entity.Structure
{
    public class UserInfo : UserInfoBase
    {
        public UserInfo(IPEndPoint remoteIPEndPoint) : base(remoteIPEndPoint)
        {
        }
    }
}