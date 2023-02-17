using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Entity.Enumerate;

namespace UniSpy.Server.NatNegotiation.Entity.Structure.Result
{
    public sealed class AddressCheckResult : CommonResultBase
    {
        public AddressCheckResult()
        {
            PacketType = ResponseType.AddressReply;
        }
    }
}
