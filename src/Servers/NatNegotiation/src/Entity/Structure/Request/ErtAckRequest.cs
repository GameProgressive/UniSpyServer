
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request
{

    public sealed class ErtAckRequest : CommonRequestBase
    {
        public ErtAckRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
