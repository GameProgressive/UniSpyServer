using QueryReport.Entity.Enumerator;

namespace QueryReport.Entity.Structure.Packet
{
    public class EchoPacket : BasePacket
    {
        public byte[] EchoMessage = System.Text.Encoding.ASCII.GetBytes("This is an echo packet from retrospy server");

        public EchoPacket(byte[] recv) : base(recv)
        {
        }

        public override byte[] GenerateResponse()
        {
            PacketType = (byte)QRPacketType.Echo;
            byte[] buffer = new byte[7 + EchoMessage.Length];
            base.GenerateResponse().CopyTo(buffer, 0);
            EchoMessage.CopyTo(buffer, 7);

            return buffer;
        }
    }
}
