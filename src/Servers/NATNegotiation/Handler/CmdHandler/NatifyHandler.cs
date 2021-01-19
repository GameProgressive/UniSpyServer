using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;
using NATNegotiation.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;

namespace NATNegotiation.Handler.CmdHandler
{
    internal sealed class NatifyHandler : NNCmdHandlerBase
    {
        private new NatifyRequest _request => (NatifyRequest)base._request;
        public NatifyHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new NatifyResult();
        }
        protected override void ResponseConstruct()
        {
            _response = new InitResponse(_request, _result);
        }
    }
}
