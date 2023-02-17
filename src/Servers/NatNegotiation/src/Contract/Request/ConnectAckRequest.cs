using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Enumerate;

namespace UniSpy.Server.NatNegotiation.Contract.Request
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