using NatNegotiation.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass.Contract;

namespace NatNegotiation.Entity.Contract
{
    public class RequestContract : RequestContractBase
    {
        public new RequestType Name => (RequestType)base.Name;
        public RequestContract(RequestType name) : base(name) { }
    }
}