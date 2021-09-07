using UniSpyLib.Abstraction.BaseClass.Contract;

namespace Chat.Entity.Contract
{
    public class RequestContract : HandlerContractBase
    {
        public new string Name => (string)base.Name;
        public RequestContract(string name) : base(name) { }
    }
}