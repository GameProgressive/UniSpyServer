using System;
using System.Collections.Generic;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;

namespace QueryReport.Entity.Structure.Response
{
    public class AddErrorResponse : QRResponseBase
    {

        public AddErrorResponse(QRRequestBase request) : base(request)
        {
            PacketType = QRPacketType.ADDError;
        }

        public AddErrorResponse(int instantKey) : base(QRPacketType.ADDError, instantKey)
        {
        }

        public override byte[] BuildResponse()
        {
            List<byte> data = new List<byte>();
            data.AddRange(base.BuildResponse());

            return data.ToArray();
        }
    }
}
