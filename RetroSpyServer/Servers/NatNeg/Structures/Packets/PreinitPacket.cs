using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.NatNeg.Structures
{
    public class PreinitPacket
    {
        public byte clientIndex;
        public byte state;
        public uint clientID;
    }
}
