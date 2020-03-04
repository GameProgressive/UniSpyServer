using QueryReport.Entity.Enumerator;

namespace QueryReport.Entity.Structure.Packet
{
    public class KeepAlivePacket : BasePacket
    {
        public KeepAlivePacket(byte[] recv) : base(recv)
        {
        }
    }
}
