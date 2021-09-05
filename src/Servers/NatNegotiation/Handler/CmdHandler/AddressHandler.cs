using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Entity.Contract;
using NatNegotiation.Entity.Enumerate;
using NatNegotiation.Entity.Structure.Request;
using NatNegotiation.Entity.Structure.Response;
using NatNegotiation.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;

namespace NatNegotiation.Handler.CmdHandler
{
    [HandlerContract(RequestType.AddressCheck)]
    internal sealed class AddressCheckHandler : CmdHandlerBase
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
