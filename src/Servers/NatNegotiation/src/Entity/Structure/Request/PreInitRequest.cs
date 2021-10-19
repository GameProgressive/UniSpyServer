using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Entity.Contract;
using NatNegotiation.Entity.Enumerate;

namespace NatNegotiation.Entity.Structure.Request
{
    [RequestContract(RequestType.PreInit)]
    public sealed class PreInitRequest : RequestBase
    {
        public int CLientIndex { get; private set; }
        public int State { get; private set; }
        public int ClientID { get; private set; }
        public PreInitRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
