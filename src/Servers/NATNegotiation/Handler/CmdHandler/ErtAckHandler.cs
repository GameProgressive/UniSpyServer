using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;
using System;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Entity.Structure.Result;

namespace NATNegotiation.Handler.CmdHandler
{

    public class ErtAckHandler : NNCmdHandlerBase
    {
        private new ErtAckRequest _request => (ErtAckRequest)base._request;
        public ErtAckHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            _result = new ErtAckResult();
        }
        protected override void DataOperation()
        {
        }
        protected override void ResponseConstruct()
        {
            _response = new InitResponse(_request, _result);
        }
    }
}
