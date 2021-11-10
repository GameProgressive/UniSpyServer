using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Request;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Result;
using System;
using System.Collections.Generic;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Response
{
    public sealed class ClientMessageResponse : ResponseBase
    {
        private new ClientMessageRequest _request => (ClientMessageRequest)base._request;
        private new ClientMessageResult _result => (ClientMessageResult)base._result;

        public ClientMessageResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            base.Build();
            List<byte> data = new List<byte>();
            data.AddRange(SendingBuffer);
            data.AddRange(BitConverter.GetBytes((int)_result.MessageKey));
            data.AddRange(_result.NatNegMessage);
            SendingBuffer = data.ToArray();
        }
    }
}
