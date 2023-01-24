using System.Collections.Generic;
using UniSpyServer.Servers.QueryReport.V2.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.V2.Entity.Enumerate;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Request;

namespace UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Response
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
