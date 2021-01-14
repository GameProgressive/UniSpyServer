using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Structure.Result;
using System.Collections.Generic;
using UniSpyLib.Abstraction.BaseClass;

namespace NATNegotiation.Entity.Structure.Response
{
    public sealed class ConnectResponse : NNResponseBase
    {
        private new ConnectResult _result => (ConnectResult)base._result;

        public ConnectResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        protected override void BuildNormalResponse()
        {
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
