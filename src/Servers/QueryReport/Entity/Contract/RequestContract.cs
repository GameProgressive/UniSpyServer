using QueryReport.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass.Contract;

namespace QueryReport.Entity.contract
{
    public class RequestContract : RequestContractBase
    {
        public new RequestType Name => (RequestType)base.Name;

        public RequestContract(RequestType name) : base(name)
        {
        }
    }
}