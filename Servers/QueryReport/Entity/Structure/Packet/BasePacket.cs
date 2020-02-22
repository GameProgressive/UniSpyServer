using System;
namespace QueryReport.Entity.Structure.Packet
{
    public class BasePacket
    {
        public static readonly byte[] MagicData = { 0xFE, 0XFD };
        public BasePacket()
        {
        }
    }
}
