using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using NATNegotiation.Entity.Enumerate;
using System;
using System.Collections.Generic;

namespace NATNegotiation.Abstraction.BaseClass
{
    /// <summary>
    /// NatNeg request base
    /// </summary>
    public class NNRequestBase : IRequest
    {
        public static readonly byte[] MagicData = { 0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2 };
        public byte Version;
        public NatPacketType PacketType { get; set; }

        object IRequest.CommandName => PacketType;

        public uint Cookie;
        public byte[] RawRequest;
        public static readonly int Size = 12;

        public NNRequestBase(byte[] rawRequest)
        {
            RawRequest = rawRequest;
        }

        public virtual bool Parse()
        {
            if (RawRequest.Length < Size)
            {
                return false;
            }

            Version = RawRequest[MagicData.Length];
            PacketType = (NatPacketType)RawRequest[MagicData.Length + 1];
            Cookie = BitConverter.ToUInt32(ByteTools.SubBytes(RawRequest, MagicData.Length + 2, 4));

            return true;
        }

        public virtual byte[] BuildResponse()
        {
            List<byte> data = new List<byte>();
            data.AddRange(MagicData);
            data.Add(Version);
            data.Add((byte)PacketType);
            data.AddRange(BitConverter.GetBytes(Cookie));

            return data.ToArray();
        }

        public NNRequestBase SetPacketType(NatPacketType type)
        {
            PacketType = type;
            return this;
        }

        object IRequest.Parse()
        {
            return Parse();
        }
    }
}
