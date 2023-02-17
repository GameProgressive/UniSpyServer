using System;
using System.Linq;
using UniSpy.Server.NatNegotiation.Enumerate;

namespace UniSpy.Server.NatNegotiation.Abstraction.BaseClass
{
    /// <summary>
    /// NatNeg request base
    /// </summary>
    public abstract class RequestBase : UniSpy.Server.Core.Abstraction.BaseClass.RequestBase
    {
        public static readonly byte[] MagicData = { 0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2 };
        public new byte[] RawRequest => (byte[])base.RawRequest;
        public byte Version { get; protected set; }
        /// <summary>
        /// Cookie is a random int, which we can not trace two clients
        /// </summary>
        /// <value></value>
        public uint Cookie { get; protected set; }
        public NatPortType PortType { get; protected set; }
        public new RequestType CommandName { get => (RequestType)base.CommandName; set => base.CommandName = value; }
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
            CommandName = (RequestType)RawRequest[7];
            Cookie = BitConverter.ToUInt32(RawRequest.Skip(8).Take(4).ToArray());
            PortType = (NatPortType)RawRequest[12];
        }
    }
}
