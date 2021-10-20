using UniSpyServer.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.NatNegotiation.Entity.Contract;
using UniSpyServer.NatNegotiation.Entity.Enumerate;
namespace UniSpyServer.NatNegotiation.Entity.Structure.Request
{
    [RequestContract(RequestType.Connect)]
    public sealed class ConnectRequest : RequestBase
    {
        public NatPortType PortType { get; set; }
        public ConnectRequest() { }
    }
}