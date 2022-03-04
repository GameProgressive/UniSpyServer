using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Result;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Response
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
