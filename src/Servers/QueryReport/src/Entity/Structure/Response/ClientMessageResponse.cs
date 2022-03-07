using System;
using System.Collections.Generic;
using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Request;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Response
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
            data.AddRange(_request.Message);
            SendingBuffer = data.ToArray();
        }
    }
}
