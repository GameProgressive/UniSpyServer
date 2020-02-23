using System;
using GameSpyLib.Encryption;

namespace QueryReport.Entity.Structure.Packet
{
    public class BaseResponsePacket
    {
        public static readonly byte[] MagicData = { 0xFE, 0XFD };
        public byte PacketType;
        public byte[] InstantKey = new byte[4]; 
    }
}
