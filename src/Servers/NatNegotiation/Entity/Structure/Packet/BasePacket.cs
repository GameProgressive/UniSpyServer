using GameSpyLib.Extensions;
using NatNegotiation.Entity.Enumerate;
using System;
using System.Collections.Generic;

namespace NatNegotiation.Entity.Structure.Packet
{
    public class BasePacket
    {
        public static readonly byte[] MagicData = { 0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2 };
        public byte Version;
        public NatPacketType PacketType { get; set; }
        public uint Cookie;

        public static readonly int Size = 12;

        public virtual bool Parse(byte[] recv)
        {
            if (recv.Length < Size)
            {
                return false;
            }

            Version = recv[MagicData.Length];
            PacketType = (NatPacketType)recv[MagicData.Length + 1];
            Cookie = BitConverter.ToUInt32(ByteTools.SubBytes(recv, MagicData.Length + 2, 4));

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

        public BasePacket SetPacketType(NatPacketType type)
        {
            PacketType = type;
            return this;
        }
    }
}
