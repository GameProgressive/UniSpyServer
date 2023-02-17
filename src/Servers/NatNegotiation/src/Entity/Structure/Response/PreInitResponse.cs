using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Entity.Structure.Request;
using UniSpy.Server.NatNegotiation.Entity.Structure.Result;

namespace UniSpy.Server.NatNegotiation.Entity.Structure.Response
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