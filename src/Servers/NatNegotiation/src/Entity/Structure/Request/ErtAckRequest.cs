
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request
{
    [RequestContract(RequestType.ErtAck)]
    public sealed class ErtAckRequest : InitRequestBase
    {
        public ErtAckRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
