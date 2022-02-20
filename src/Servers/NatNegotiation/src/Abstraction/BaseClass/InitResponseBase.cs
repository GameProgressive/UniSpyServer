using System.Collections.Generic;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;

namespace UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass
{
    public abstract class InitResponseBase : ResponseBase
    {
        private new InitRequestBase _request => (InitRequestBase)base._request;
        private new InitResultBase _result => (InitResultBase)base._result;
        public InitResponseBase(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
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
            data.AddRange(_result.RemoteIPBytes);
            data.AddRange(_result.RemotePortBytes);
            SendingBuffer = data.ToArray();
        }
    }
}