using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Contract.Request;
using UniSpy.Server.NatNegotiation.Contract.Result;

namespace UniSpy.Server.NatNegotiation.Contract.Response
{
    public sealed class InitResponse : CommonResponseBase
    {
        private new InitRequest _request => (InitRequest)base._request;
        private new InitResult _result => (InitResult)base._result;
        public InitResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }
    }
}
