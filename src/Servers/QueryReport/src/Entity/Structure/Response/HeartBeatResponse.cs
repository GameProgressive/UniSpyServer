using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Result;
using System.Collections.Generic;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Response
{
    public sealed class HeartBeatResponse : ResponseBase
    {
        private new HeartBeatResult _result => (HeartBeatResult)base._result;
        private static readonly byte[] Challenge = { 0x54, 0x54, 0x54, 0x00, 0x00 };
        private static readonly byte[] Spliter = { 0x00, 0x00, 0x00, 0x00 };
        public HeartBeatResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            base.Build();
            List<byte> data = new List<byte>();
            data.AddRange(SendingBuffer);
            data.AddRange(Challenge);
            data.AddRange(HtonsExtensions.IPStringToBytes(_result.RemoteIP));
            data.AddRange(Spliter);
            data.AddRange(HtonsExtensions.PortToIntBytes(_result.RemotePort));
            SendingBuffer = data.ToArray();
        }
    }
}
