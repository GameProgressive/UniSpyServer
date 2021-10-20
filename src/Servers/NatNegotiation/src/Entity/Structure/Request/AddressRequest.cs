
using UniSpyServer.NatNegotiation.Entity.Contract;
using UniSpyServer.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.NatNegotiation.Entity.Structure.Request
{
    [RequestContract(RequestType.AddressCheck)]
    public sealed class AddressRequest : InitRequestBase
    {
        public AddressRequest(byte[] rawRequest) : base(rawRequest)
        {
        }

    }
}
