using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;

namespace NATNegotiation.Handler.CommandHandler
{
    public class NatifyHandler : NNCommandHandlerBase
    {
        protected new NatifyRequest _request;
        public NatifyHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _request = (NatifyRequest)request;
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer =
                new NatifyResponse(_request, _session.RemoteEndPoint).BuildResponse();
        }
    }
}
