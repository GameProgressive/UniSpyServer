using GameSpyLib.Encryption;

namespace QueryReport.Entity.Structure.Packet
{
    public class BaseResponsePacket
    {
        public static readonly byte[] MagicData = { 0xFE, 0XFD };
        public byte PacketType;
        public byte[] InstantKey = new byte[4];

        public BaseResponsePacket(byte[] recv)
        {
            ByteTools.SubBytes(recv, 1, 4).CopyTo(InstantKey, 0);
        }
        public BaseResponsePacket() { }

        public virtual byte[] GenerateResponse()
        {
            byte[] buffer = new byte[7];
            buffer[0] = MagicData[0];
            buffer[1] = MagicData[1];
            buffer[2] = PacketType;
            InstantKey.CopyTo(buffer, 3);
            return buffer;
        }
    }
}
