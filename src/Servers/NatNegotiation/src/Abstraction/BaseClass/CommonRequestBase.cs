using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass
{
    public abstract class CommonRequestBase : RequestBase
    {
        public NatClientIndex? ClientIndex { get; protected set; }
        public byte? UseGamePort { get; protected set; }
        protected CommonRequestBase(byte[] rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            base.Parse();
            ClientIndex = (NatClientIndex)RawRequest[13];
            UseGamePort = RawRequest[14];
        }
    }
}
