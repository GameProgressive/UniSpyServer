using System.Net;
using NATNegotiation.Application;
using NATNegotiation.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass.Redis;
using UniSpyLib.Extensions;
namespace NATNegotiation.Entity.Structure.Misc
{
    public class NatUserInfoRedisKey : UniSpyRedisKeyBase
    {
        public IPEndPoint RemoteIPEndPoint { get; set; }
        public NatPortType PortType { get; set; }
        public uint Cookie { get; set; }
        public NatUserInfoRedisKey()
        {
        }
    }
}
