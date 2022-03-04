using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Result;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Response
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