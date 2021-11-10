using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass
{
    public class ResultBase : UniSpyResultBase
    {
        public NatPacketType? PacketType { get; set; }
        public ResultBase()
        {
        }
    }
}
