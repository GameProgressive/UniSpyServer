using System;
using System.Collections.Generic;
using UniSpy.Server.QueryReport.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.Contract.Request;
using UniSpy.Server.QueryReport.Contract.Result;

namespace UniSpy.Server.QueryReport.Contract.Response
{
    public sealed class HeartBeatResponse : ResponseBase
    {
        private new HeartBeatRequest _request => (HeartBeatRequest)base._request;
        private new HeartBeatResult _result => (HeartBeatResult)base._result;
        private static readonly byte[] Challenge = { 0x54, 0x54, 0x54, 0x00, 0x00 };
        private static readonly byte[] Spliter = { 0x00, 0x00, 0x00, 0x00 };
        public HeartBeatResponse(HeartBeatRequest request, HeartBeatResult result) : base(request, result)
        {
        }

        public override void Build()
        {
            base.Build();
            List<byte> data = new List<byte>();
            data.AddRange(SendingBuffer);
            data.AddRange(Challenge);
            data.AddRange(_result.RemoteIPEndPoint.Address.GetAddressBytes());
            data.AddRange(Spliter);
            data.AddRange(BitConverter.GetBytes((int)_result.RemoteIPEndPoint.Port));
            SendingBuffer = data.ToArray();
        }
    }
}
