using System.Collections.Generic;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;

namespace UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass
{
    public abstract class CommonResponseBase : ResponseBase
    {
        private new CommonResultBase _result => (CommonResultBase)base._result;
        private new CommonRequestBase _request => (CommonRequestBase)base._request;
        public CommonResponseBase(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            base.Build();
            List<byte> data = new List<byte>();
            data.AddRange(SendingBuffer);
            data.Add((byte)_request.PortType);
            data.Add((byte)_request.ClientIndex);
            data.Add((byte)_request.UseGamePort);
            data.AddRange(_result.RemoteIPAddressBytes);
            data.AddRange(_result.RemotePortBytes);
            SendingBuffer = data.ToArray();
        }
    }
}