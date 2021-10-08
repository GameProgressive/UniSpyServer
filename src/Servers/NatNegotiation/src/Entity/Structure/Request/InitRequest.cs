using NatNegotiation.Entity.Contract;
using NatNegotiation.Entity.Enumerate;

namespace NatNegotiation.Entity.Structure.Request
{
    [RequestContract(RequestType.Init)]
    internal sealed class InitRequest : InitRequestBase
    {
        public InitRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
