using NATNegotiation.Abstraction.BaseClass;
using PresenceConnectionManager.Abstraction.BaseClass;

namespace NATNegotiation.Entity.Structure.Request
{
    [Command(15)]
    internal sealed class PreInitRequest : NNRequestBase
    {
        public int CLientIndex;
        public int State;
        public int ClientID;
        public PreInitRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
