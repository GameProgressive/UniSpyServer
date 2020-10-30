using GameSpyLib.Extensions;
using QueryReport.Entity.Enumerate;
using System;
using System.Collections.Generic;

namespace QueryReport.Entity.Abstraction.BaseClass
{
    public class BasePacket
    {
        public static readonly byte[] MagicData = { 0xFE, 0XFD };
        public QRPacketType PacketType { get; protected set; }
        public int InstantKey { get; protected set; }

        public virtual bool Parse(byte[] recv)
        {
            if (recv.Length < 3)
                return false;

            PacketType = (QRPacketType)recv[0];
            InstantKey = BitConverter.ToInt32(ByteTools.SubBytes(recv, 1, 4));
            return true;
        }

        public virtual byte[] BuildResponse()
        {
            List<byte> data = new List<byte>();
            data.AddRange(MagicData);
            data.Add((byte)PacketType);
            data.AddRange(BitConverter.GetBytes(InstantKey));

            return data.ToArray();
        }

        public BasePacket SetPacketType(QRPacketType packetType)
        {
            PacketType = packetType;
            return this;
        }

        public BasePacket SetInstanceKey(int instantKey)
        {
            InstantKey = instantKey;
            return this;
        }

    }
}
