using UniSpyLib.Abstraction.BaseClass;

namespace NatNegotiation.Entity.Structure.Request
{
    [Command((byte)0)]
    internal sealed class InitRequest : NNInitRequestBase
    {
        public InitRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
