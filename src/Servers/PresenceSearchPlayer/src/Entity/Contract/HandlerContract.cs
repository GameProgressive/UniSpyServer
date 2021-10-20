using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Contract;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Contract
{
    public class HandlerContract : HandlerContractBase
    {
        public new string Name => (string)base.Name;
        public HandlerContract(string name) : base(name)
        {
        }
    }
}