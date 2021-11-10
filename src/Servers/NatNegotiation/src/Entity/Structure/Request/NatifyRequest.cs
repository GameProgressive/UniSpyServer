using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request
{
    [RequestContract(RequestType.NatifyRequest)]
    public sealed class NatifyRequest : InitRequestBase
    {
        public NatifyRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
