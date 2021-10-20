
using UniSpyServer.NatNegotiation.Entity.Contract;
using UniSpyServer.NatNegotiation.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.NatNegotiation.Entity.Structure.Request
{
    [RequestContract(RequestType.ErtAck)]
    public sealed class ErtAckRequest : InitRequestBase
    {
        public ErtAckRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
