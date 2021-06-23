
using UniSpyLib.Abstraction.BaseClass;

namespace NatNegotiation.Entity.Structure.Request
{
    [Command((byte)3)]
    internal sealed class ErtAckRequest : NNInitRequestBase
    {
        public ErtAckRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
