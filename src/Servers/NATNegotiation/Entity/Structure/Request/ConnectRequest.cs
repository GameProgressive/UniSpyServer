using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using PresenceConnectionManager.Abstraction.BaseClass;

namespace NATNegotiation.Entity.Structure.Request
{
    [Command(5)]
    internal sealed class ConnectRequest : NNRequestBase
    {
        public NatPortType PortType { get; set; }
        public ConnectRequest() { }
    }
}