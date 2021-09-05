using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Entity.Contract;
using NatNegotiation.Entity.Enumerate;
namespace NatNegotiation.Entity.Structure.Request
{
    [RequestContract(RequestType.Connect)]
    internal sealed class ConnectRequest : RequestBase
    {
        public NatPortType PortType { get; set; }
        public ConnectRequest() { }
    }
}