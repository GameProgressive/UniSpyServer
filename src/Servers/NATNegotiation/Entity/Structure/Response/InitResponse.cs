using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Result;
using System.Collections.Generic;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;

namespace NATNegotiation.Entity.Structure.Response
{
    internal sealed class InitResponse : NNResponseBase
    {
        private new InitRequest _request => (InitRequest)base._request;
        private new InitResult _result => (InitResult)base._result;
        public InitResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
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
