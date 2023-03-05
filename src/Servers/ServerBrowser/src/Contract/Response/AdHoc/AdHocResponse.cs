using System;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.Contract.Request;

namespace UniSpy.Server.ServerBrowser.Aggregate.Packet.Response
{
    public class AdHocResponse : ResponseBase
    {
        public new HeartBeatRequest _request => (HeartBeatRequest)base._request;
        public AdHocResponse(HeartBeatRequest request) : base(request, null)
        {
        }

        public override void Build()
        {
            throw new NotImplementedException();
        }

        // public byte[] GenerateByteArray()
        // {
        //     //the 2 bytes are length of this request
        //     byte[] byteLength = BitConverter.GetBytes((ushort)(_keyValueData.Length + 2));
        //     List<byte> data = new List<byte>();
        //     data.AddRange(byteLength);
        //     data.AddRange(_keyValueData);

        //     return data.ToArray();
        // }
    }
}
