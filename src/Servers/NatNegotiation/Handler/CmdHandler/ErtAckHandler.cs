using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Entity.Contract;
using NatNegotiation.Entity.Enumerate;
using NatNegotiation.Entity.Structure.Request;
using NatNegotiation.Entity.Structure.Response;
using NatNegotiation.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace NatNegotiation.Handler.CmdHandler
{
    [HandlerContract(RequestType.ErtAck)]
    internal sealed class ErtAckHandler : CmdHandlerBase
    {
        private new ErtAckRequest _request => (ErtAckRequest)base._request;
        public ErtAckHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new ErtAckResult();
        }
        protected override void ResponseConstruct()
        {
            _response = new InitResponse(_request, _result);
        }
    }
}
