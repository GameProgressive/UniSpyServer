using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;
using NATNegotiation.Entity.Structure.Result;

namespace NATNegotiation.Handler.CmdHandler
{
    public class NatifyHandler : NNCmdHandlerBase
    {
        protected new NatifyRequest _request { get { return (NatifyRequest)base._request; } }
        public NatifyHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            _result = new NatifyResult();
        }

        protected override void ResponseConstruct()
        {
            _response = new NatifyResponse(_request, _result);
        }


    }
}
