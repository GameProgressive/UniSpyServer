using System.Collections.Generic;
using UniSpy.Server.QueryReport.V2.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V2.Enumerate;
using UniSpy.Server.QueryReport.V2.Contract.Request;

namespace UniSpy.Server.QueryReport.V2.Contract.Response
{
    public sealed class AvaliableResponse : ResponseBase
    {
        public static readonly byte[] ResponsePrefix = { 0xfe, 0xfd, 0x09, 0x00, 0x00, 0x00 };

        public AvaliableResponse(AvaliableRequest request) : base(request, null)
        {
        }

        public override void Build()
        {
            List<byte> data = new List<byte>();

            data.AddRange(ResponsePrefix);
            data.Add((byte)ServerAvailability.Available);
            // NOTE: Change this if you want to make the server not avaliable.
            SendingBuffer = data.ToArray();
        }
    }
}
