using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Response
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
