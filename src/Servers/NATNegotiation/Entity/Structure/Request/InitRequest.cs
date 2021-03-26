namespace NATNegotiation.Entity.Structure.Request
{
    internal sealed class InitRequest : NNInitRequestBase
    {
        public InitRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
