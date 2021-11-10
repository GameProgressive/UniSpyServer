using UniSpyServer.Servers.ServerBrowser.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Contract;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Contract
{
    public class HandlerContract : HandlerContractBase
    {
        public new RequestType Name => (RequestType)base.Name;
        public HandlerContract(RequestType name) : base(name)
        {
        }
    }
}