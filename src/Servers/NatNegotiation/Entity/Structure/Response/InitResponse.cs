using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Entity.Structure.Request;
using NatNegotiation.Entity.Structure.Result;
using System.Collections.Generic;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;

namespace NatNegotiation.Entity.Structure.Response
{
    internal sealed class InitResponse : ResponseBase
    {
        private new InitRequest _request => (InitRequest)base._request;
        private new InitResult _result => (InitResult)base._result;
        public InitResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            base.Build();
            List<byte> data = new List<byte>();
            data.AddRange(SendingBuffer);
            data.Add((byte)_request.PortType);
            data.Add(_request.ClientIndex);
            data.Add(_request.UseGamePort);
            data.AddRange(HtonsExtensions.IPStringToBytes(_result.LocalIP));
            data.AddRange(HtonsExtensions.UshortPortToBytes(_result.LocalPort));
            SendingBuffer = data.ToArray();
        }
    }
}
