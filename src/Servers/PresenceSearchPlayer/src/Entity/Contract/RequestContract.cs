using UniSpyLib.Abstraction.BaseClass.Contract;

namespace PresenceSearchPlayer.Entity.Contract
{
    public class RequestContract : RequestContractBase
    {
        public new string Name => (string)base.Name;
        public RequestContract(string name) : base(name)
        {
        }
    }
}