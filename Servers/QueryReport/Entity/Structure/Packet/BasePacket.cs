using GameSpyLib.Extensions;
using QueryReport.Entity.Enumerator;
using System;
using System.Collections.Generic;

namespace QueryReport.Entity.Structure.Packet
{
    public class BasePacket
    {
        public static readonly byte[] MagicData = { 0xFE, 0XFD };
        public QRPacketType PacketType { get; protected set; }
        public int InstantKey { get; protected set; }

        public virtual void Parse(byte[] recv)
        {
            PacketType = (QRPacketType)recv[0];
            InstantKey = BitConverter.ToInt32(ByteTools.SubBytes(recv, 1, 4));
        }

        public virtual byte[] GenerateResponse()
        {
            List<byte> data = new List<byte>();
            data.AddRange(MagicData);
            data.Add((byte)PacketType);
            data.AddRange(BitConverter.GetBytes(InstantKey));

            return data.ToArray();
        }
    }
}
