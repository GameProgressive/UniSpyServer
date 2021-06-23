using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Entity.Structure.Result;
using System.Collections.Generic;
using UniSpyLib.Abstraction.BaseClass;

namespace NatNegotiation.Entity.Structure.Response
{
    internal sealed class ConnectResponse : NNResponseBase
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
            data.AddRange(_result.RemoteIPAddress);
            data.AddRange(_result.RemotePort);
            data.Add((byte)_result.GotYourData);
            data.Add((byte)_result.Finished);

            SendingBuffer = data.ToArray();
        }
    }
}
