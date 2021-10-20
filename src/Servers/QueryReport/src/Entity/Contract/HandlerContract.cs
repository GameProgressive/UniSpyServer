using UniSpyServer.QueryReport.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Contract;

namespace UniSpyServer.QueryReport.Entity.contract
{
    public class HandlerContract : HandlerContractBase
    {
        public new RequestType Name => (RequestType)base.Name;

        public HandlerContract(RequestType name) : base(name)
        {
        }
    }
}