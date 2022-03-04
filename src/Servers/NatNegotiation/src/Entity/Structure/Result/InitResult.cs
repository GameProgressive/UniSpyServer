using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Result
{
    public sealed class InitResult : CommonResultBase
    {
        public InitResult()
        {
            PacketType = ResponseType.InitAck;
        }
    }
}
