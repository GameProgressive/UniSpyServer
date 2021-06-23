using NATNegotiation.Entity.Enumerate;
using PresenceConnectionManager.Abstraction.BaseClass;

namespace NATNegotiation.Entity.Structure.Request
{
    [Command(12)]
    internal sealed class NatifyRequest : NNInitRequestBase
    {
        public NatifyRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
