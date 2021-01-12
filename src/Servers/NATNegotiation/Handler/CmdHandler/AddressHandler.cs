using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;
using NATNegotiation.Entity.Structure.Result;

namespace NATNegotiation.Handler.CmdHandler
{
<<<<<<< HEAD
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
=======
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
>>>>>>> c309f4b009e514a1d1f13db4317bdf0d8c2e4797
        }
    }
}
