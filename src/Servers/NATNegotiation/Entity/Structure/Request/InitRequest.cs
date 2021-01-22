namespace NATNegotiation.Entity.Structure.Request
{
    internal sealed class InitRequest : InitRequestBase
    {
        public InitRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
