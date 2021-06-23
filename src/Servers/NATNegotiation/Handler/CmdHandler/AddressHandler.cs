using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Entity.Structure.Request;
using NatNegotiation.Entity.Structure.Response;
using NatNegotiation.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace NatNegotiation.Handler.CmdHandler
{
    [Command((byte)10)]
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
