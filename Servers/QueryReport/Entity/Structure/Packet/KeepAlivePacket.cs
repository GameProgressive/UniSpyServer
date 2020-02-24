using QueryReport.Entity.Enumerator;

namespace QueryReport.Entity.Structure.Packet
{
    public class KeepAlivePacket : BaseResponsePacket
    {
        public KeepAlivePacket(byte[] recv) : base(recv)
        {
            PacketType = (byte)QRPacketType.KeepAlive;
        }
    }
}
