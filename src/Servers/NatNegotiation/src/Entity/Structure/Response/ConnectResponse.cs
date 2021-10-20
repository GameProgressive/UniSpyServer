using UniSpyServer.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.NatNegotiation.Entity.Structure.Result;
using System.Collections.Generic;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.NatNegotiation.Entity.Structure.Response
{
    public sealed class ConnectResponse : ResponseBase
    {
        private new ConnectResult _result => (ConnectResult)base._result;

        public ConnectResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
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
