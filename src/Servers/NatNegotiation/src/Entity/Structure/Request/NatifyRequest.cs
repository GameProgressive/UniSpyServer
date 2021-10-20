using UniSpyServer.NatNegotiation.Entity.Contract;
using UniSpyServer.NatNegotiation.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.NatNegotiation.Entity.Structure.Request
{
    [RequestContract(RequestType.NatifyRequest)]
    public sealed class NatifyRequest : InitRequestBase
    {
        public NatifyRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
