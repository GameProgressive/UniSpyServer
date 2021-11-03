using UniSpyServer.Servers.ServerBrowser.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Contract;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Contract
{
    public class RequestContract : RequestContractBase
    {
        public new RequestType Name => (RequestType)base.Name;
        public RequestContract(RequestType name) : base(name)
        {
        }
    }
}