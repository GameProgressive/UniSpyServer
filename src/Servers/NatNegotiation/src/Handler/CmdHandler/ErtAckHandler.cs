using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Contract.Request;
using UniSpy.Server.NatNegotiation.Contract.Response;
using UniSpy.Server.NatNegotiation.Contract.Result;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.NatNegotiation.Handler.CmdHandler
{

    public sealed class ErtAckHandler : CmdHandlerBase
    {
        private new ErtAckRequest _request => (ErtAckRequest)base._request;
        private new ErtAckResult _result { get => (ErtAckResult)base._result; set => base._result = value; }
        public ErtAckHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new ErtAckResult();
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
