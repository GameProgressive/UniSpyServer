using QueryReport.Entity.Enumerator;
using System.Text;

namespace QueryReport.Entity.Structure.Packet
{
    public class ClientMessagePacket:BaseResponsePacket
    {
        string _message;
        public ClientMessagePacket(string message)
        {
            _message = message;
            PacketType = (byte)QRPacketType.ClientMessage;

        }
        public override byte[] GenerateResponse()
        {
            byte[] msg = Encoding.ASCII.GetBytes(_message);
            byte[] buffer = new byte[7+msg.Length];
            buffer[0] = MagicData[0];
            buffer[1] = MagicData[1];
            buffer[2] = PacketType;
            InstantKey.CopyTo(buffer, 3);
            msg.CopyTo(buffer, 7);
            return buffer;
        }
    }
}
