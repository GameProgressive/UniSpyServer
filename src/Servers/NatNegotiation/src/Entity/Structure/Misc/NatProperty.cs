using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Misc
{
    public record NatProperty
    {
        public NatType NatType { get; init; }
        public NatPromiscuty Promiscuity { get; init; }
        public NatPortMappingScheme PortMapping { get; init; }
        public NatProperty()
        {
        }

    }
}