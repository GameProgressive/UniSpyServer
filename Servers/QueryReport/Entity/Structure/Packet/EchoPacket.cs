using System;
using QueryReport.Entity.Enumerator;

namespace QueryReport.Entity.Structure.Packet
{
    public class EchoPacket:BaseResponsePacket
    {
        public byte[] EchoMessage = System.Text.Encoding.ASCII.GetBytes("This is an echo packet from retrospy server");

        public EchoPacket(byte[] recv) : base(recv)
        {
            PacketType = (byte)QRPacketType.Echo;
        }
        public override byte[] GenerateResponse()
        {
            byte[] buffer = new byte[7 + EchoMessage.Length];
            base.GenerateResponse().CopyTo(buffer, 0);
            EchoMessage.CopyTo(buffer, 7);
            return buffer;
        }
    }
}
