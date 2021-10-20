using UniSpyServer.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.NatNegotiation.Entity.Structure.Result;
using System.Collections.Generic;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.NatNegotiation.Entity.Structure.Response
{
    public sealed class InitResponse : InitResponseBase
    {
        private new InitRequest _request => (InitRequest)base._request;
        private new InitResult _result => (InitResult)base._result;
        public InitResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
    }
}
