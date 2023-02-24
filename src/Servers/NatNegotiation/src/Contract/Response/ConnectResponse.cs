using System.Collections.Generic;
using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Contract.Request;
using UniSpy.Server.NatNegotiation.Contract.Result;

namespace UniSpy.Server.NatNegotiation.Contract.Response
{
    public sealed class ConnectResponse : ResponseBase
    {
        private new ConnectRequest _request => (ConnectRequest)base._request;
        private new ConnectResult _result => (ConnectResult)base._result;
        public ConnectResponse(ConnectRequest request, ConnectResult result) : base(request, result)
        {
        }
        public override void Build()
        {
            base.Build();
            List<byte> data = new List<byte>();
            data.AddRange(SendingBuffer);
            data.AddRange(_result.RemoteIPAddressBytes);
            data.AddRange(_result.RemotePortBytes);
            data.Add((byte)_result.GotYourData);
            data.Add((byte)_result.Finished);
            SendingBuffer = data.ToArray();
        }
    }
}
