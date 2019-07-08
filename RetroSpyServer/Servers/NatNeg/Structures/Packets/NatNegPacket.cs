using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RetroSpyServer.Servers.NatNeg.Structures
{
    public class NatNegPacket
    {
        
        public byte version;
        public  byte packettype;
        public uint  cookie;

        [StructLayout(LayoutKind.Explicit, Size = 8)]
        struct Packet
	{
            [FieldOffset(0)]
            InitPacket Init;
            [FieldOffset(0)]
            ConnectPacket Connect;
            [FieldOffset(0)]
            ReportPacket Report;
            [FieldOffset(0)]
            PreinitPacket Preinit;
    }

}
}
