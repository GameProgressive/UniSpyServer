using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request
{
    [RequestContract(RequestType.Connect)]
    public sealed class ConnectRequest : RequestBase
    {
        public NatPortType PortType { get; set; }
        public ConnectRequest() { }
    }
}