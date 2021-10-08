using QueryReport.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass.Contract;

namespace QueryReport.Entity.contract
{
    public class HandlerContract : HandlerContractBase
    {
        public new RequestType Name => (RequestType)base.Name;

        public HandlerContract(RequestType name) : base(name)
        {
        }
    }
}