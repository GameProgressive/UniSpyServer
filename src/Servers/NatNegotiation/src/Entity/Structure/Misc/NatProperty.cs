using UniSpy.Server.NatNegotiation.Entity.Enumerate;

namespace UniSpy.Server.NatNegotiation.Entity.Structure.Misc
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