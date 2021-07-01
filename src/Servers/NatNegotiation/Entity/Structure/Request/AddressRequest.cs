
using UniSpyLib.Abstraction.BaseClass;

namespace NatNegotiation.Entity.Structure.Request
{
    [Command((byte)10)]
    internal sealed class AddressRequest : NNInitRequestBase
    {
        public AddressRequest(byte[] rawRequest) : base(rawRequest)
        {
        }

    }
}
