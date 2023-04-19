using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Contract.Request;
using UniSpy.Server.NatNegotiation.Contract.Response;
using UniSpy.Server.NatNegotiation.Contract.Result;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.NatNegotiation.Application;

namespace UniSpy.Server.NatNegotiation.Handler.CmdHandler
{

    public sealed class NatifyHandler : CmdHandlerBase
    {
        private new NatifyRequest _request => (NatifyRequest)base._request;
        private new NatifyResult _result { get => (NatifyResult)base._result; set => base._result = value; }
        public NatifyHandler(Client client, NatifyRequest request) : base(client, request)
        {
            _result = new NatifyResult();
        }
        protected override void DataOperation()
        {
            _result.RemoteIPEndPoint = _client.Connection.RemoteIPEndPoint;
        }
        protected override void ResponseConstruct()
        {
            _response = new InitResponse(_request, _result);
        }
    }
}
