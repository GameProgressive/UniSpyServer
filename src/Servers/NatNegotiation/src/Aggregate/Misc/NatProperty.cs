using UniSpy.Server.NatNegotiation.Enumerate;

namespace UniSpy.Server.NatNegotiation.Aggregate.Misc
{
    public record NatProperty
    {
        public NatType NatType { get; set; }
        public NatPortMappingScheme PortMapping { get; set; }
        public int PortIncrement { get; set; }
        public bool isPortRestricted { get; set; }
        public bool isIPRestricted { get; set; }
        public NatProperty()
        {
            NatType = NatType.Unknown;
            PortMapping = NatPortMappingScheme.Unrecognized;
            isPortRestricted = false;
            isIPRestricted = false;
        }
    }
}