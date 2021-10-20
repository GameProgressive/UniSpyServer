using UniSpyServer.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.NatNegotiation.Entity.Structure.Result
{
    public sealed class ErtAckResult : InitResultBase
    {
        public ErtAckResult()
        {
            PacketType = NatPacketType.ErtAck;
        }
    }
}
