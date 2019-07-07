using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.NatNeg.Structures
{
    public class InitPacket
    {
        public byte porttype;
        public byte clientindex;
        public byte usegameport;
        public uint localip;
        public ushort localport;

    }
}
