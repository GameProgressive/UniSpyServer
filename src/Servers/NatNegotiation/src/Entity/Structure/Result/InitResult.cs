using UniSpyServer.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.NatNegotiation.Entity.Enumerate;
using System.Net;

namespace UniSpyServer.NatNegotiation.Entity.Structure.Result
{
    public sealed class InitResult : InitResultBase
    {
        public InitResult()
        {
            PacketType = NatPacketType.InitAck;
        }
    }
}
