using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;


namespace NATNegotiation.Handler.CmdHandler
{
    public class AddressCheckHandler : NNCommandHandlerBase
    {
        protected new AddressRequest _request { get { return (AddressRequest)base._request; } }
        public AddressCheckHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void ResponseConstruct()
        {
            _sendingBuffer = new AddressResponse(_request, _session.RemoteEndPoint).BuildResponse();
        }
    }
}
