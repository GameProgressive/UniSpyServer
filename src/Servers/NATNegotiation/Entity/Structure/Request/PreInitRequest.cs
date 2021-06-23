using NatNegotiation.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace NatNegotiation.Entity.Structure.Request
{
    [Command((byte)15)]
    internal sealed class PreInitRequest : NNRequestBase
    {
        public int CLientIndex { get; private set; }
        public int State { get; private set; }
        public int ClientID { get; private set; }
        public PreInitRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
