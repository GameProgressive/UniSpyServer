using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Entity.Structure.Request;
using NatNegotiation.Entity.Structure.Result;
using System.Collections.Generic;
using UniSpyLib.Abstraction.BaseClass;

namespace NatNegotiation.Entity.Structure.Response
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
