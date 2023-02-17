using System;
using UniSpy.Server.NatNegotiation.Entity.Enumerate;

namespace UniSpy.Server.NatNegotiation.Abstraction.BaseClass
{
    public abstract class CommonRequestBase : RequestBase
    {
        public NatClientIndex ClientIndex { get; protected set; }
        public bool UseGamePort { get; protected set; }
        protected CommonRequestBase(byte[] rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            base.Parse();
            ClientIndex = (NatClientIndex)RawRequest[13];
            UseGamePort = Convert.ToBoolean(RawRequest[14]);
        }
    }
}
