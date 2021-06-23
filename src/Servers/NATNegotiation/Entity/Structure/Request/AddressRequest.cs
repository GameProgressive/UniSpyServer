using PresenceConnectionManager.Abstraction.BaseClass;

namespace NATNegotiation.Entity.Structure.Request
{
    [Command(10)]
    internal sealed class AddressRequest : NNInitRequestBase
    {
        public AddressRequest(byte[] rawRequest) : base(rawRequest)
        {
        }

    }
}
