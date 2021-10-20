using UniSpyServer.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.NatNegotiation.Entity.Contract;
using UniSpyServer.NatNegotiation.Entity.Enumerate;
using UniSpyServer.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.NatNegotiation.Entity.Structure.Response;
using UniSpyServer.NatNegotiation.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.NatNegotiation.Handler.CmdHandler
{
    [HandlerContract(RequestType.ErtAck)]
    public sealed class ErtAckHandler : CmdHandlerBase
    {
        private new ErtAckRequest _request => (ErtAckRequest)base._request;
        private new ErtAckResult _result { get => (ErtAckResult)base._result; set => base._result = value; }
        public ErtAckHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new ErtAckResult();
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
