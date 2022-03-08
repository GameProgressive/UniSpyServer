
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request
{
    [RequestContract(RequestType.AddressCheck)]
    public sealed class AddressCheckRequest : CommonRequestBase
    {
        public AddressCheckRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
