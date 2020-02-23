using System;
using GameSpyLib.Encryption;
using QueryReport.Entity.Enumerator;

namespace QueryReport.Entity.Structure.Packet
{
    public class KeepAlivePacket : BaseResponsePacket
    {
        public KeepAlivePacket(byte[] recv)
        {
            ByteTools.SubBytes(recv, 1, 4).CopyTo(InstantKey, 0);
        }

        public byte[] GenerateResponse()
        {
            byte[] buffer = new byte[7];
            buffer[0] = MagicData[0];
            buffer[1] = MagicData[1];
            buffer[2] =(byte) QRPacketType.KeepAlive;
            InstantKey.CopyTo(buffer, 3);
            return buffer;
        }
    }
}
