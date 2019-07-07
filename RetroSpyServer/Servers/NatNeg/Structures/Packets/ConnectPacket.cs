using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.NatNeg.Structures
{
    public  class ConnectPacket
    {
        public uint remoteIP;
        public ushort remotePort;
        public byte gotyourdata;
        public byte finished;
    }
}
