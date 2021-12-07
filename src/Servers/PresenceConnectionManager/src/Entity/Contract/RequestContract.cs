using UniSpyServer.UniSpyLib.Abstraction.Contract;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract
{
    public class RequestContract : RequestContractBase
    {
        public new string Name => (string)base.Name;
        public RequestContract(string name) : base(name)
        {
        }
    }
}