
using NatNegotiation.Entity.Contract;
using NatNegotiation.Entity.Enumerate;

namespace NatNegotiation.Entity.Structure.Request
{
    [RequestContract(RequestType.AddressCheck)]
    public sealed class AddressRequest : InitRequestBase
    {
        public AddressRequest(byte[] rawRequest) : base(rawRequest)
        {
        }

    }
}
