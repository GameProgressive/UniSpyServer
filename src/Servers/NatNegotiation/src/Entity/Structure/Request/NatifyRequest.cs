using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request
{

    public sealed class NatifyRequest : CommonRequestBase
    {
        public NatifyRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
