using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Entity.Enumerate;
using System.Net;

namespace NatNegotiation.Entity.Structure.Result
{
    internal class InitResult : InitResultBase
    {
        public InitResult()
        {
            PacketType = NatPacketType.InitAck;
        }
    }
}
