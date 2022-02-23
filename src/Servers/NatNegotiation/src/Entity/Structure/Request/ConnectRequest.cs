using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request
{
    [RequestContract(RequestType.Connect)]
    public sealed class ConnectRequest : RequestBase
    {
        public new byte? Version { get => base.Version; set => base.Version = value; }
        public new uint? Cookie { get => base.Cookie; set => base.Cookie = value; }
        public new NatPortType? PortType { get => base.PortType; set => base.PortType = value; }
        public ConnectRequest() { }
    }
}