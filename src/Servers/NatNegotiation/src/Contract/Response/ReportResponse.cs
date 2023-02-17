using System.Collections.Generic;
using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Contract.Request;

namespace UniSpy.Server.NatNegotiation.Contract.Response
{
    public sealed class ReportResponse : ResponseBase
    {
        private new ReportRequest _request => (ReportRequest)base._request;
        public ReportResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            base.Build();
            var data = new List<byte>();
            data.AddRange(SendingBuffer);
            // at least 9 bytes more
            // the rest bytes did not read by natneg client, but you have to make total bytes length > 21 to pass initpacket check
            data.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00});
            SendingBuffer = data.ToArray();
        }
    }
}
