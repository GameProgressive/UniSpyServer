using System;
using System.Linq;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass
{
    /// <summary>
    /// NatNeg request base
    /// </summary>
    public abstract class RequestBase : UniSpyLib.Abstraction.BaseClass.RequestBase
    {
        public static readonly byte[] MagicData = { 0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2 };
        public new byte[] RawRequest => (byte[])base.RawRequest;
        public byte? Version { get; protected set; }
        public uint? Cookie { get; protected set; }
        public NatPortType? PortType { get; protected set; }
        // public byte? ClientIndex { get; protected set; }
        // public byte? UseGamePort { get; protected set; }
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
            // ClientIndex = RawRequest[13];
            // UseGamePort = RawRequest[14];
        }
    }
}
