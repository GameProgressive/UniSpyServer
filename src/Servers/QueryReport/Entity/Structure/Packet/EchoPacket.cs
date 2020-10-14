using QueryReport.Entity.Enumerator;
using System.Collections.Generic;
using System.Text;

namespace QueryReport.Entity.Structure.Packet
{
    public class EchoPacket : BasePacket
    {
        public string Message { get; protected set; }

        public EchoPacket() : base()
        {
            Message = "RetroSpy echo!";
        }

        public override byte[] BuildResponse()
        {
            List<byte> data = new List<byte>();
            PacketType = QRPacketType.Echo;

            data.AddRange(base.BuildResponse());
            data.AddRange(Encoding.ASCII.GetBytes(Message));

            return data.ToArray();
        }
    }
}
