using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;
using NATNegotiation.Entity.Structure.Result;

namespace NATNegotiation.Handler.CmdHandler
{
    internal sealed class AddressCheckHandler : NNCommandHandlerBase
    {
        private new AddressRequest _request => (AddressRequest)base._request;
        public AddressCheckHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void RequestCheck()
        {
            _result = new NNDefaultResult();
        }
        protected override void ResponseConstruct()
        {
            _response = new AddressResponse(_request, _result);
            _sendingBuffer = new AddressResponse(_request, _session.RemoteEndPoint).BuildResponse();
        }
    }
}
