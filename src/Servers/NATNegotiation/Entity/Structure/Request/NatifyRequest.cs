namespace NATNegotiation.Entity.Structure.Request
{
    internal sealed class NatifyRequest : InitRequestBase
    {
        public NatifyRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
