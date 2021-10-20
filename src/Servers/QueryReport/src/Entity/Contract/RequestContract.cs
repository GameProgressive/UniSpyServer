using UniSpyServer.QueryReport.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Contract;

namespace UniSpyServer.QueryReport.Entity.contract
{
    public class RequestContract : RequestContractBase
    {
        public new RequestType Name => (RequestType)base.Name;

        public RequestContract(RequestType name) : base(name)
        {
        }
    }
}