using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Enumerate;

namespace UniSpy.Server.NatNegotiation.Contract.Result
{
    public sealed class NatifyResult : CommonResultBase
    {
        public NatifyResult()
        {
            PacketType = ResponseType.ErtTest;
        }
    }
}
