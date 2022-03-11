using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Response;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.NatNegotiation.Handler.CmdHandler
{
    [HandlerContract(RequestType.NatifyRequest)]
    public sealed class NatifyHandler : CmdHandlerBase
    {
        private new NatifyRequest _request => (NatifyRequest)base._request;
        private new NatifyResult _result { get => (NatifyResult)base._result; set => base._result = value; }
        public NatifyHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new NatifyResult();
        }
        protected override void DataOperation()
        {
            _result.PublicIPEndPoint = _client.Session.RemoteIPEndPoint;
        }
        protected override void ResponseConstruct()
        {
            _response = new InitResponse(_request, _result);
        }
    }
}
