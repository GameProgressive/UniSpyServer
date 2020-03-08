using System;
using GameSpyLib.Encryption;

namespace ServerBrowser.Entity.Structure.Packet.Request
{
    public class ServerInfoPacket
    {
        public ushort MessageLength { get; protected set; }
        public byte[] IPByte { get; protected set; }
        public byte[] PortByte { get; protected set; }
        public uint IP { get { return Convert.ToUInt32(IPByte); } }
        public ushort Port { get { return Convert.ToUInt16(PortByte); } }

        public ServerInfoPacket(byte[] recv)
        {
            IPByte = new byte[4];
            PortByte = new byte[2];
            byte[] lengthByte = new byte[2];
            ByteTools.SubBytes(recv, 0, 2).CopyTo(lengthByte, 0);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(lengthByte);
            MessageLength = BitConverter.ToUInt16(lengthByte,0);

            ByteTools.SubBytes(recv, 2, 4).CopyTo(IPByte, 0);
            ByteTools.SubBytes(recv, 7, 2).CopyTo(PortByte, 0);
        }
    }
}
