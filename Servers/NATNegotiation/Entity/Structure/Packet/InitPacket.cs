using GameSpyLib.Encryption;
using NatNegotiation.Entity.Enumerator;
using System;

namespace NatNegotiation.Entity.Structure.Packet
{
    public class InitPacket : BasePacket
    {
        private static readonly int InitPacketSize = 9;
        public static readonly int PacketSize = BasePacketSize + InitPacketSize;

        public byte PortType;
        public byte ClientIndex;
        public byte UseGamePort;
        public uint LocalIp;
        public ushort LocalPort;

        public InitPacket(byte[] data) : base(data)
        {
            PortType = data[13];//02
            ClientIndex = data[14];//00
            UseGamePort = data[15];//00
            LocalIp = BitConverter.ToUInt32(ByteExtensions.SubBytes(data, 16, sizeof(uint)));//00 - 00 - 00 - 00
            LocalPort = BitConverter.ToUInt16(ByteExtensions.SubBytes(data, 20, sizeof(ushort)));//00 - 00
        }
        public byte[] CreateReplyPacket()
        {
            byte[] TempBytes = new byte[GetReplyPacketSize()];
            NatNegInfo.MagicData.CopyTo(TempBytes, 0);
            TempBytes[magicDataLen] = Version;
            TempBytes[magicDataLen + 1] = (byte)NatPacketType.InitAck;
            BitConverter.GetBytes(Cookie).CopyTo(TempBytes, magicDataLen + 2);

            TempBytes[BasePacketSize] = PortType;
            TempBytes[BasePacketSize + 1] = ClientIndex;
            TempBytes[BasePacketSize + 2] = UseGamePort;
            BitConverter.GetBytes(LocalIp).CopyTo(TempBytes, BasePacketSize + 3);
            BitConverter.GetBytes(LocalPort).CopyTo(TempBytes, BasePacketSize + 7);
            return TempBytes;
        }

    }
}
