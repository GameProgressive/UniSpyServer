using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;
using System;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Entity.Structure.Result;

namespace NATNegotiation.Handler.CmdHandler
{
<<<<<<< HEAD
    internal sealed class ErtAckHandler : NNCommandHandlerBase
=======
    public class ErtAckHandler : NNCmdHandlerBase
>>>>>>> c309f4b009e514a1d1f13db4317bdf0d8c2e4797
    {
        private new ErtAckRequest _request => (ErtAckRequest)base._request;
        public ErtAckHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {

        }

        protected override void RequestCheck()
        {
            _result = new ErtAckResult();
        }

        protected override void ResponseConstruct()
        {
            _response = new ErtAckResponse(_request, _result);
        }

    }
}
