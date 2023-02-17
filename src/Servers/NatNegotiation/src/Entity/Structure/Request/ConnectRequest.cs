using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Entity.Enumerate;
namespace UniSpy.Server.NatNegotiation.Entity.Structure.Request
{

    public sealed class ConnectRequest : RequestBase
    {
        public new byte Version { get => base.Version; init => base.Version = value; }
        public new uint Cookie { get => base.Cookie; init => base.Cookie = value; }
        public NatClientIndex ClientIndex { get; init; }
        public ConnectRequest() { }
    }
}