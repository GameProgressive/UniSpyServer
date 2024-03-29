using System;
using System.Collections.Generic;
using UniSpy.Server.QueryReport.V2.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V2.Contract.Request;

namespace UniSpy.Server.QueryReport.V2.Contract.Response
{
    public sealed class ClientMessageResponse : ResponseBase
    {
        private new ClientMessageRequest _request => (ClientMessageRequest)base._request;
        public ClientMessageResponse(ClientMessageRequest request) : base(request, null)
        {
        }

        public override void Build()
        {
            base.Build();
            List<byte> data = new List<byte>();
            data.AddRange(SendingBuffer);
            data.AddRange(BitConverter.GetBytes((int)_request.MessageKey));
            data.AddRange(_request.NatNegMessage);
            SendingBuffer = data.ToArray();
        }
    }
}
