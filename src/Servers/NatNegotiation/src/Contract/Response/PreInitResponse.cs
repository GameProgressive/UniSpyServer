using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Contract.Request;
using UniSpy.Server.NatNegotiation.Contract.Result;

namespace UniSpy.Server.NatNegotiation.Contract.Response
{
    public sealed class PreInitResponse : CommonResponseBase
    {
        private new PreInitRequest _request => (PreInitRequest)base._request;
        private new PreInitResult _result => (PreInitResult)base._result;
        public PreInitResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            base.Build();
        }
    }
}