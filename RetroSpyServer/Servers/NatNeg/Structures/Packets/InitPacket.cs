using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.NatNeg.Structures
{
    public class InitPacket
    {
        public byte portType;
        public byte clientIndex;
        public byte useGamePort;
        public uint localIp;
        public ushort localPort;
    }
}
