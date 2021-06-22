using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;

namespace NATNegotiation.Entity.Structure.Request
{
    internal sealed class ConnectRequest : NNRequestBase
    {
        public NatPortType PortType { get; set; }
        public ConnectRequest() { }
    }
}