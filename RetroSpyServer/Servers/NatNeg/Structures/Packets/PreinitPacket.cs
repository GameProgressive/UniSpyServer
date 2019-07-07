using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.NatNeg.Structures
{
    public class PreinitPacket
    {
        public byte clientindex;
        public byte state;
        public uint clientID;
    }
}
