using System;
using System.Collections.Generic;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace QueryReport.Entity.Structure.Response
{
    internal sealed class AvaliableResponse : QRResponseBase
    {
        public static readonly byte[] Response = { 0xfe, 0xfd, 0x09, 0x00, 0x00, 0x00 };

        public AvaliableResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
            List<byte> buffer = new List<byte>();

            buffer.AddRange(Response);
            buffer.Add((byte)ServerAvailability.Available);
            // NOTE: Change this if you want to make the server not avaliable.
            SendingBuffer = buffer.ToArray();
        }
    }
}
