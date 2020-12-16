using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;
using System;
using NATNegotiation.Entity.Structure;

namespace NATNegotiation.Handler.CmdHandler
{
    public class ErtAckHandler : NNCommandHandlerBase
    {
        protected new ErtAckRequest _request { get { return (ErtAckRequest)base._request; } }
        public ErtAckHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {

        }

        protected override void ConstructResponse()
        {
            _sendingBuffer =
                new ErtAckResponse(_request, _session.RemoteEndPoint).BuildResponse();
        }
    }
}
