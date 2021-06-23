using PresenceConnectionManager.Abstraction.BaseClass;

namespace NATNegotiation.Entity.Structure.Request
{
    [Command(0)]
    internal sealed class InitRequest : NNInitRequestBase
    {
        public InitRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
