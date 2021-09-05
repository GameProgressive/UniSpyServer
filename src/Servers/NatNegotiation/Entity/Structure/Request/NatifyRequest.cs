using NatNegotiation.Entity.Contract;
using NatNegotiation.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace NatNegotiation.Entity.Structure.Request
{
    [RequestContract(RequestType.NatifyRequest)]
    internal sealed class NatifyRequest : InitRequestBase
    {
        public NatifyRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
