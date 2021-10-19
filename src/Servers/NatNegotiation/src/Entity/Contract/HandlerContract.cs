using NatNegotiation.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass.Contract;

namespace NatNegotiation.Entity.Contract
{
    public class HandlerContract : HandlerContractBase
    {
        public new RequestType Name => (RequestType)base.Name;
        public HandlerContract(RequestType name) : base(name) { }


    }
}