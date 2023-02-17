
using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;

namespace UniSpy.Server.NatNegotiation.Contract.Request
{

    public sealed class AddressCheckRequest : CommonRequestBase
    {
        public AddressCheckRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
