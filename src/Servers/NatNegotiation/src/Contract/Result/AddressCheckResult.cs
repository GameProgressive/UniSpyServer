using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Enumerate;

namespace UniSpy.Server.NatNegotiation.Contract.Result
{
    public sealed class AddressCheckResult : CommonResultBase
    {
        public AddressCheckResult()
        {
            PacketType = ResponseType.AddressReply;
        }
    }
}
