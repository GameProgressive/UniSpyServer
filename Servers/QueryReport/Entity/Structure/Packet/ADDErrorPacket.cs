using QueryReport.Entity.Enumerator;
using System;

namespace QueryReport.Entity.Structure.Packet
{
    public class ADDErrorPacket : BaseResponsePacket
    {
        public ADDErrorPacket(byte[] recv) : base(recv)
        {
            PacketType = (byte)QRPacketType.ADDError;
            throw new NotImplementedException();
        }
    }
}
