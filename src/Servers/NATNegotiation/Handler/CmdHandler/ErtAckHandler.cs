using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;

namespace NATNegotiation.Handler.CmdHandler
{
    public class ErtAckHandler : NNCommandHandlerBase
    {
        protected new ErtAckRequest _request;
        public ErtAckHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _request = (ErtAckRequest)request;
        }

        protected override void DataOperation()
        {
            _session.UserInfo.Parse(_request);
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer =
                new ErtAckResponse(_request, _session.RemoteEndPoint).BuildResponse();
        }
    }
}
