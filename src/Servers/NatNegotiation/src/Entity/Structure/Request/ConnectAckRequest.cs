using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request
{

    public sealed class ConnectAckRequest : RequestBase
    {
        public NatClientIndex ClientIndex { get; private set; }
        public ConnectAckRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            base.Parse();
            ClientIndex = (NatClientIndex)RawRequest[13];
        }
    }
}