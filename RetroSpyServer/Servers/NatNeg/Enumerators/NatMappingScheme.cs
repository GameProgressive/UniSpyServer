using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.NatNeg.Enumerators
{
   public enum NatMappingScheme
    {
        unrecognized,
        private_as_public,
        consistent_port,
        incremental,
        mixed,
        NUM_MAPPING_SCHEMES
    }
}
