
using NatNegotiation.Entity.Contract;
using NatNegotiation.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace NatNegotiation.Entity.Structure.Request
{
    [RequestContract(RequestType.ErtAck)]
    internal sealed class ErtAckRequest : InitRequestBase
    {
        public ErtAckRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
