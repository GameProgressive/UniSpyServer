
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request
{
    [RequestContract(RequestType.AddressCheck)]
    public sealed class AddressRequest : InitRequestBase
    {
        public AddressRequest(byte[] rawRequest) : base(rawRequest)
        {
        }

    }
}
