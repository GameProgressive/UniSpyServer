using QueryReport.Entity.Enumerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryReport.Entity.Structure.Packet
{
    public class ClientMessagePacket : BasePacket
    {
        public byte[] GenerateResponse(string message)
        {
            List<byte> data = new List<byte>();
            //we need to change packet type to client message then send
            PacketType = QRPacketType.ClientMessage;
            data.AddRange(base.GenerateResponse());
            data.AddRange(Encoding.ASCII.GetBytes(message));
            return data.ToArray();
        }
    }
}
