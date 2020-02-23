using System;
using QueryReport.Entity.Enumerator;

namespace QueryReport.Entity.Structure.Packet
{
    public class EchoPacket:BaseResponsePacket
    {
        public byte[] EchoMessage = System.Text.Encoding.ASCII.GetBytes("This is an echo packet from retrospy server");

        public EchoPacket()
        {
            PacketType = (byte)QRPacketType.Echo;
        }
    }
}
