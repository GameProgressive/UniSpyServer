using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.Contract;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Contract
{
    public class HandlerContract : HandlerContractBase
    {
        public new RequestType Name => (RequestType)base.Name;
        public HandlerContract(RequestType name) : base(name) { }


    }
}