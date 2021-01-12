using NATNegotiation.Abstraction.BaseClass;

namespace NATNegotiation.Entity.Structure.Request
{
    internal class PreInitRequest : NNRequestBase
    {
        public int CLientIndex;
        public int State;
        public int ClientID;

        public new static readonly int Size = NNRequestBase.Size + 6;

        public PreInitRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
