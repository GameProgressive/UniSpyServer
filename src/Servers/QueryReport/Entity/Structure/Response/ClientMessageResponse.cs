using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Structure.Result;
using System;
using System.Collections.Generic;
using UniSpyLib.Abstraction.BaseClass;

namespace QueryReport.Entity.Structure.Request
{
    internal sealed class ClientMessageResponse : QRResponseBase
    {
        private new ClientMessageRequest _request => (ClientMessageRequest)base._request;
        private new ClientMessageResult _result => (ClientMessageResult)base._result;

        public ClientMessageResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            List<byte> data = new List<byte>();
            data.AddRange(SendingBuffer);
            data.AddRange(BitConverter.GetBytes((int)_result.MessageKey));
            data.AddRange(_result.NatNegMessage);
            SendingBuffer = data.ToArray();
        }
    }
}
