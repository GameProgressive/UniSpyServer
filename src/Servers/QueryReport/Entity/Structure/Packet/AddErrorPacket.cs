using QueryReport.Entity.Enumerate;
using System;
using System.Collections.Generic;

namespace QueryReport.Entity.Abstraction.BaseClass
{
    public class AddErrorPacket : BasePacket
    {
        public AddErrorPacket() : base()
        {
            throw new NotImplementedException();
        }

        public override byte[] BuildResponse()
        {
            List<byte> data = new List<byte>();
            PacketType = QRPacketType.ADDError;
            data.AddRange(base.BuildResponse());

            return data.ToArray();
        }
    }
}
