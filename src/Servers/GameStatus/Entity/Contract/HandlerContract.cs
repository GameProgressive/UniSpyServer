using UniSpyLib.Abstraction.BaseClass.Contract;
namespace GameStatus.Entity.Contract
{
    public class HandlerContract : HandlerContractBase
    {
        public new string Name => (string)base.Name;

        public HandlerContract(string name) : base(name)
        {
        }

    }
}