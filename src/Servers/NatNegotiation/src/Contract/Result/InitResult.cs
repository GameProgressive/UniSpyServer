using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Enumerate;

namespace UniSpy.Server.NatNegotiation.Contract.Result
{
    public sealed class InitResult : CommonResultBase
    {
        public InitResult()
        {
            PacketType = ResponseType.InitAck;
        }
    }
}
