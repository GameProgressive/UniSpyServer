using System;
using System.Linq;
using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Entity.Enumerate;

namespace UniSpy.Server.NatNegotiation.Entity.Structure.Request
{
    
    public sealed class PreInitRequest : RequestBase
    {
        public PreInitState? State { get; private set; }
        public uint? TargetCookie { get; private set; }
        public PreInitRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            base.Parse();
            State = (PreInitState)RawRequest[12];
            TargetCookie = BitConverter.ToUInt32(RawRequest.Skip(13).Take(4).ToArray());
        }
    }
}
