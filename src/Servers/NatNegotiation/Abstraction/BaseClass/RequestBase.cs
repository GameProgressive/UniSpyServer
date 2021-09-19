using NatNegotiation.Entity.Enumerate;
using System;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;

namespace NatNegotiation.Abstraction.BaseClass
{
    /// <summary>
    /// NatNeg request base
    /// </summary>
    public abstract class RequestBase : UniSpyRequestBase
    {
        public static readonly byte[] MagicData = { 0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2 };
        public new byte[] RawRequest => (byte[])base.RawRequest;
        public byte Version { get; set; }
        public uint Cookie { get; set; }
        public RequestBase(byte[] rawRequest) : base(rawRequest)
        {
        }
        public RequestBase() { }
        public override void Parse()
        {
            if (RawRequest.Length < 12)
            {
                return;
            }

            Version = RawRequest[6];
            Cookie = BitConverter.ToUInt32(RawRequest.Skip(8).Take(4).ToArray());
        }
    }
}
