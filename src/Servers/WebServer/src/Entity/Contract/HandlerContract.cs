using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Contract;

namespace UniSpyServer.Servers.WebServer.Entity.Contract
{
    public class HandlerContract : HandlerContractBase
    {
        public HandlerContract(string name) : base(name)
        {
        }
    }
}