using UniSpyServer.Servers.QueryReport.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.Contract;

namespace UniSpyServer.Servers.QueryReport.Entity.contract
{
    public class HandlerContract : HandlerContractBase
    {
        public new RequestType Name => (RequestType)base.Name;

        public HandlerContract(RequestType name) : base(name)
        {
        }
    }
}