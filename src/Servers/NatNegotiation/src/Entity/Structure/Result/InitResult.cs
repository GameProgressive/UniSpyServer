using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Entity.Enumerate;
using System.Net;

namespace NatNegotiation.Entity.Structure.Result
{
    internal sealed class InitResult : InitResultBase
    {
        public InitResult()
        {
            PacketType = NatPacketType.InitAck;
        }
    }
}
