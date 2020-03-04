using System;
using QueryReport.Entity.Enumerator;

namespace QueryReport.Entity.Structure.Packet
{
    public class AddErrorPacket : BasePacket
    {
        public AddErrorPacket(byte[] recv) : base(recv)
        {
            PacketType = (byte)QRPacketType.ADDError;
            throw new NotImplementedException();
        }
    }
}
