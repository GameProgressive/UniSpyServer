using ServerBrowser.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass.Contract;

namespace ServerBrowser.Entity.Contract
{
    public class RequestContract : RequestContractBase
    {
        public new RequestType Name => (RequestType)base.Name;
        public RequestContract(RequestType name) : base(name)
        {
        }
    }
}