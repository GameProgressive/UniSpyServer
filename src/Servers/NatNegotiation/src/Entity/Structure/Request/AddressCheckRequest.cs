
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request
{

    public sealed class AddressCheckRequest : CommonRequestBase
    {
        public AddressCheckRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
