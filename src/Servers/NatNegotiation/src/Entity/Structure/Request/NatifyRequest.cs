using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request
{
    
    public sealed class NatifyRequest : CommonRequestBase
    {
        public NatifyRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
