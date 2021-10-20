using UniSpyServer.NatNegotiation.Entity.Contract;
using UniSpyServer.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.NatNegotiation.Entity.Structure.Request
{
    [RequestContract(RequestType.Init)]
    public sealed class InitRequest : InitRequestBase
    {
        public InitRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
