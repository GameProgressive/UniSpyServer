using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request
{
    [RequestContract(RequestType.Init)]
    public sealed class InitRequest : InitRequestBase
    {
        public InitRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
