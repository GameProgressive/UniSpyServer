using System.Collections.Generic;
using System.Text;
using QueryReport.Entity.Enumerator;

namespace QueryReport.Entity.Structure.Packet
{
    public class EchoPacket : BasePacket
    {
        public string EchoMessage { get; protected set; }

        public EchoPacket() : base()
        {
            EchoMessage = "This is an echo packet";
        }

        public override byte[] GenerateResponse()
        {
            List<byte> data = new List<byte>();

            PacketType = QRPacketType.Echo;

            data.AddRange(base.GenerateResponse());
            data.AddRange(Encoding.ASCII.GetBytes(EchoMessage));

            return data.ToArray();
        }
    }
}
