using QueryReport.Entity.Enumerator;
using System.Collections.Generic;
using System.Text;

namespace QueryReport.Entity.Structure.Packet
{
    public class ClientMessagePacket : BasePacket
    {
        public byte[] Message { get; protected set; }

        public override byte[] BuildResponse()
        {
            List<byte> data = new List<byte>();
            //we need to change packet type to client message then send
            PacketType = QRPacketType.ClientMessage;

            data.AddRange(base.BuildResponse());
            data.AddRange(Message);
            return data.ToArray();
        }

        public ClientMessagePacket SetMessage(byte[] message)
        {
            Message = message;
            return this;
        }
    }
}
