using System;
using System.Collections.Generic;
using System.Text;

namespace NATNegotiation.Structure.Packet
{
    public class PreInitPacket : BasePacket
    {
        private static readonly int PreInitPacketSize = 6;
        private static readonly int PacketSize = BasePacketSize + PreInitPacketSize;
        public PreInitPacket(byte[] data):base(data)
        {

        }
    }
}
