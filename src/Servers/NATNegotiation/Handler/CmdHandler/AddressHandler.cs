using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;
using NATNegotiation.Entity.Structure.Result;

namespace NATNegotiation.Handler.CmdHandler
{
    public class AddressCheckHandler : NNCmdHandlerBase
    {
        protected new AddressRequest _request
        {
            get { return (AddressRequest)base._request; }
        }
        protected new AddressResult _result
        {
            get { return (AddressResult)base._result; }
            set { base._result = value; }
        }
        public AddressCheckHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            _result = new AddressResult();
        }

        protected override void ResponseConstruct()
        {
            _response = new AddressResponse(_request, _result);
        }
    }
}
