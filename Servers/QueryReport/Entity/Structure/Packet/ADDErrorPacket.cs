using System;
using QueryReport.Entity.Enumerator;

namespace QueryReport.Entity.Structure.Packet
{
    public class ADDErrorPacket : BaseResponsePacket
    {
        public ADDErrorPacket(byte[] recv)
        {
            PacketType = (byte)QRPacketType.ADDError;
        }
    }
}
