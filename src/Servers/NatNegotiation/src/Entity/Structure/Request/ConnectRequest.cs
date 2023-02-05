using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request
{

    public sealed class ConnectRequest : RequestBase
    {
        public new byte Version { get => base.Version; init => base.Version = value; }
        public new uint Cookie { get => base.Cookie; init => base.Cookie = value; }
        public NatClientIndex ClientIndex { get; init; }
        public ConnectRequest() { }
    }
}