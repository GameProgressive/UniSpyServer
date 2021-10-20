using UniSpyServer.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.NatNegotiation.Entity.Contract;
using UniSpyServer.NatNegotiation.Entity.Enumerate;
using UniSpyServer.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.NatNegotiation.Entity.Structure.Response;
using UniSpyServer.NatNegotiation.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.NatNegotiation.Handler.CmdHandler
{
    [HandlerContract(RequestType.NatifyRequest)]
    public sealed class NatifyHandler : CmdHandlerBase
    {
        private new NatifyRequest _request => (NatifyRequest)base._request;
        private new NatifyResult _result { get => (NatifyResult)base._result; set => base._result = value; }
        public NatifyHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new NatifyResult();
        }
        protected override void DataOperation()
        {
            _result.RemoteIPEndPoint = _session.RemoteIPEndPoint;
        }
        protected override void ResponseConstruct()
        {
            _response = new InitResponse(_request, _result);
        }
    }
}
