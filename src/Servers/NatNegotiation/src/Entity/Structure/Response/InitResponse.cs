using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Entity.Structure.Request;
using UniSpy.Server.NatNegotiation.Entity.Structure.Result;

namespace UniSpy.Server.NatNegotiation.Entity.Structure.Response
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
