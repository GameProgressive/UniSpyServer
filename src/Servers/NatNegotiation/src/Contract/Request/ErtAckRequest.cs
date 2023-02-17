
using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;

namespace UniSpy.Server.NatNegotiation.Contract.Request
{

    public sealed class ErtAckRequest : CommonRequestBase
    {
        public ErtAckRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
