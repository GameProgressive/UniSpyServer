using System;
using System.Collections.Generic;
using QueryReport.Entity.Enumerator;

namespace QueryReport.Entity.Structure.Packet
{
    public class AddErrorPacket : BasePacket
    {
        public AddErrorPacket() : base()
        {
            throw new NotImplementedException();
        }

        public override byte[] GenerateResponse()
        {
            List<byte> data = new List<byte>();
            PacketType = QRPacketType.ADDError;
            data.AddRange(base.GenerateResponse());

            return data.ToArray();
        }
    }
}
