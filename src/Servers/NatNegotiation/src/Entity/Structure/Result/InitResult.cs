using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Result
{
    public sealed class InitResult : InitResultBase
    {
        public InitResult()
        {
            PacketType = NatPacketType.InitAck;
        }
    }
}
