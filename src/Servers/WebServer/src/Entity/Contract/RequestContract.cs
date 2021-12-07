using UniSpyServer.UniSpyLib.Abstraction.Contract;

namespace UniSpyServer.Servers.WebServer.Entity.Contract
{
    public class RequestContract : RequestContractBase
    {
        public RequestContract(string name) : base(name)
        {
        }
    }
}