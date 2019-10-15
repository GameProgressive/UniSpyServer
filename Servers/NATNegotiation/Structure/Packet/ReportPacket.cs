using GameSpyLib.Extensions;
using NATNegotiation.Enumerator;
using System;

namespace NATNegotiation.Structure.Packet
{
    public class ReportPacket : BasePacket
    {
        private static readonly int ReportPacketSize = 61;
        public static readonly int PacketSize = BasePacketSize + ReportPacketSize;

        public byte PortType;
        public byte ClientIndex;
        public byte NegResult;
        public NatType NatType; //int
        public NatMappingScheme NatMappingScheme; //int
        public byte[] GameName = new byte[50];

        public ReportPacket(byte[] data) : base(data)
        {
            PortType = data[13];
            ClientIndex = data[14];
            NegResult = data[15];

            byte[] tempNatType = ByteExtensions.SubBytes(data, 17, sizeof(int));
            NatType = (NatType)BitConverter.ToInt32(tempNatType, 0);

            byte[] tempNatMappingScheme = ByteExtensions.SubBytes(data, 19, sizeof(int));
            NatMappingScheme = (NatMappingScheme)BitConverter.ToInt32(tempNatMappingScheme, 0);

            Array.Copy(data, 23, GameName, 0, 50);
        }

        public byte[] CreateReplyPacket()
        {
            byte[] TempBytes = new byte[GetReplyPacketSize()];
            NatNegInfo.MagicData.CopyTo(TempBytes, 0);
            TempBytes[magicDataLen] = Version;
            TempBytes[magicDataLen + 1] = (byte)PacketType;
            BitConverter.GetBytes(Cookie).CopyTo(TempBytes, magicDataLen + 2);

            TempBytes[BasePacketSize] = PortType;
            TempBytes[BasePacketSize + 1] = ClientIndex;
            TempBytes[BasePacketSize + 2] = NegResult;
            BitConverter.GetBytes((int)NatType).CopyTo(TempBytes, BasePacketSize + 3);
            BitConverter.GetBytes((int)NatMappingScheme).CopyTo(TempBytes, BasePacketSize + 7);
            GameName.CopyTo(TempBytes, BasePacketSize + 11);

            return TempBytes;
        }
    }
}
