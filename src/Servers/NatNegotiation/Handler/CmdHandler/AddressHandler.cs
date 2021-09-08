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
        private new AddressResult _result { get => (AddressResult)base._result; set => base._result = value; }

        public AddressCheckHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new AddressResult();
        }
        protected override void DataOperation()
        {
            _result.RemoteIPEndPoint = _session.RemoteIPEndPoint;
        }
        protected override void ResponseConstruct()
        {
            _response = new InitResponse(_request, _result);
        }
    }
}
