using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.QueryReport.Entity
{
    public class UserInfo : UserInfoBase
    {
        public uint? InstantKey { get; set; }
        public UserInfo(IPEndPoint remoteIPEndPoint) : base(remoteIPEndPoint)
        {
        }
    }
}