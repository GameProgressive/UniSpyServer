using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;
using NATNegotiation.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;

namespace NATNegotiation.Handler.CmdHandler
{

    public class ErtAckHandler : NNCmdHandlerBase
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
