using NatNegotiation.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace NatNegotiation.Entity.Structure.Request
{
    [Command((byte)12)]
    internal sealed class NatifyRequest : NNInitRequestBase
    {
        public NatifyRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
