using System;
using System.Collections.Generic;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;

namespace QueryReport.Entity.Structure.Response
{
    public class AvaliableResponse : QRResponseBase
    {
        public static readonly byte[] Response = { 0xfe, 0xfd, 0x09, 0x00, 0x00, 0x00 };

        public override byte[] BuildResponse()
        {
            List<byte> buffer = new List<byte>();

            buffer.AddRange(Response);
            buffer.Add((byte)ServerAvailability.Available);
            // NOTE: Change this if you want to make the server not avaliable.
            return buffer.ToArray();
        }
    }
}
