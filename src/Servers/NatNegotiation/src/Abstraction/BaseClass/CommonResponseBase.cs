using System;
using System.Collections.Generic;

namespace UniSpy.Server.NatNegotiation.Abstraction.BaseClass
{
    public abstract class CommonResponseBase : ResponseBase
    {
        private new CommonResultBase _result => (CommonResultBase)base._result;
        private new CommonRequestBase _request => (CommonRequestBase)base._request;
        public CommonResponseBase(UniSpy.Server.Core.Abstraction.BaseClass.RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            base.Build();
            List<byte> data = new List<byte>();
            data.AddRange(SendingBuffer);
            data.Add((byte)_request.PortType);
            data.Add((byte)_request.ClientIndex);
            data.Add(Convert.ToByte(_request.UseGamePort));
            data.AddRange(_result.RemoteIPAddressBytes);
            data.AddRange(_result.RemotePortBytes);
            SendingBuffer = data.ToArray();
        }
    }
}