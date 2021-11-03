using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using System.Net;

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
