namespace NatNegotiation.Entity.Structure.Packet
{
    public class PreInitPacket : BasePacket
    {
        private static readonly int PreInitPacketSize = 6;
        private static readonly int PacketSize = BasePacketSize + PreInitPacketSize;
        public PreInitPacket(byte[] data) : base(data)
        {

        }
    }
}
