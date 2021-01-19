using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;
using NATNegotiation.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;

namespace NATNegotiation.Handler.CmdHandler
{
    internal sealed class AddressCheckHandler : NNCmdHandlerBase
    {
        private new AddressRequest _request => (AddressRequest)base._request;

        public AddressCheckHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new AddressResult();
        }
        protected override void ResponseConstruct()
        {
            _response = new InitResponse(_request, _result);
        }
    }
}
