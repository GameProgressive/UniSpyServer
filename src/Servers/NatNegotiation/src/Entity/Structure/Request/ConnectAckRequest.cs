using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request
{
    [RequestContract(RequestType.ConnectAck)]
    public class ConnectAckRequest : RequestBase
    {
        public ConnectAckRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}