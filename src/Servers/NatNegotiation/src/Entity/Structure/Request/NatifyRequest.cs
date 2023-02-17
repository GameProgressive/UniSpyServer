using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;

namespace UniSpy.Server.NatNegotiation.Entity.Structure.Request
{

    public sealed class NatifyRequest : CommonRequestBase
    {
        public NatifyRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
