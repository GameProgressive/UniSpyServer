namespace UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass
{
    public abstract class CommonRequestBase : RequestBase
    {
        public byte? ClientIndex { get; protected set; }
        public byte? UseGamePort { get; protected set; }
        protected CommonRequestBase(byte[] rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            base.Parse();
            ClientIndex = RawRequest[13];
            UseGamePort = RawRequest[14];
        }
    }
}
