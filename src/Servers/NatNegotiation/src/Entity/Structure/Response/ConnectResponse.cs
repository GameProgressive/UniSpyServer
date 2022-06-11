using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Result;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Response
{
    public sealed class ConnectResponse : ResponseBase
    {
        private new ConnectResult _result => (ConnectResult)base._result;
        public ConnectResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            base.Build();
            List<byte> data = new List<byte>();
            data.AddRange(SendingBuffer);
            data.AddRange(_result.RemoteIPAddressBytes);
            data.AddRange(_result.RemotePortBytes);
            Debug.WriteLine($"port:{BitConverter.ToUInt16(_result.RemotePortBytes.Reverse().ToArray())}");
            data.Add((byte)_result.GotYourData);
            data.Add((byte)_result.Finished);
            SendingBuffer = data.ToArray();
        }
    }
}
