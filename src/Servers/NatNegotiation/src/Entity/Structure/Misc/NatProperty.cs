using System.Collections.Generic;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Misc
{
    public record NatProperty
    {
        public NatType NatType { get; set; }
        public NatPortMappingScheme PortMapping { get; set; }
        public List<int> PortIncrement { get; set; }
        public bool isPortRestricted { get; set; }
        public bool isIPRestricted { get; set; }
        public NatProperty()
        {
            NatType = NatType.Unknown;
            PortMapping = NatPortMappingScheme.Unrecognized;
            PortIncrement = new List<int>();
            isPortRestricted = false;
            isIPRestricted = false;
        }
    }
}