namespace NATNegotiation.Entity.Structure.Request
{
    internal sealed class ErtAckRequest : NNInitRequestBase
    {
        public ErtAckRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
