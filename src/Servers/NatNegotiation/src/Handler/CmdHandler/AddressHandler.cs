using UniSpyServer.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.NatNegotiation.Entity.Contract;
using UniSpyServer.NatNegotiation.Entity.Enumerate;
using UniSpyServer.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.NatNegotiation.Entity.Structure.Response;
using UniSpyServer.NatNegotiation.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.NatNegotiation.Handler.CmdHandler
{
    [HandlerContract(RequestType.AddressCheck)]
    public sealed class AddressCheckHandler : CmdHandlerBase
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
