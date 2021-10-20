using UniSpyServer.QueryReport.Abstraction.BaseClass;
using UniSpyServer.QueryReport.Entity.Enumerate;
using System.Collections.Generic;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.QueryReport.Entity.Structure.Response
{
    public sealed class AvaliableResponse : ResponseBase
    {
        public static readonly byte[] ResponsePrefix = { 0xfe, 0xfd, 0x09, 0x00, 0x00, 0x00 };

        public AvaliableResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
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
