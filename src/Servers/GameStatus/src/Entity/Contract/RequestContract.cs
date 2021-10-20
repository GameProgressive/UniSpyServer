using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Contract;
namespace UniSpyServer.GameStatus.Entity.Contract
{
    public class RequestContract : RequestContractBase
    {
        public new string Name => (string)base.Name;

        public RequestContract(string name) : base(name)
        {
        }

    }
}