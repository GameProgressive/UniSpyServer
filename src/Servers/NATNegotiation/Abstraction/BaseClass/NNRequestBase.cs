using NATNegotiation.Entity.Enumerate;
using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;

namespace NATNegotiation.Abstraction.BaseClass
{
    /// <summary>
    /// NatNeg request base
    /// </summary>
    public abstract class NNRequestBase : UniSpyRequestBase
    {
        public static readonly byte[] MagicData = { 0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2 };
        public static readonly int Size = 12;

        //public NatPacketType PacketType { get; set; }
        public new NatPacketType CommandName
        {
            get => (NatPacketType)base.CommandName;
            set => base.CommandName = value;
        }
        public new byte[] RawRequest => (byte[])base.RawRequest;
        public new NNErrorCode ErrorCode
        {
            get => (NNErrorCode)base.ErrorCode;
            protected set => base.ErrorCode = value;
        }
        public byte Version;
        public uint Cookie;

        public NNRequestBase(byte[] rawRequest) : base(rawRequest)
        {
            ErrorCode = NNErrorCode.NoError;
        }

        public override void Parse()
        {
            if (RawRequest.Length < Size)
            {
                ErrorCode = NNErrorCode.Parse;
                return;
            }

            Version = RawRequest[MagicData.Length];
            CommandName = (NatPacketType)RawRequest[MagicData.Length + 1];
            Cookie = BitConverter.ToUInt32(ByteTools.SubBytes(RawRequest, MagicData.Length + 2, 4));
        }
    }
}
