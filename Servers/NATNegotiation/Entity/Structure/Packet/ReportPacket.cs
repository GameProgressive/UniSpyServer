using GameSpyLib.Encryption;
using NatNegotiation.Entity.Enumerator;
using System;

namespace NatNegotiation.Entity.Structure.Packet
{
    public class ReportPacket :BasePacket
    {
        public new static readonly int Size = BasePacket.Size + 61;

        public byte PortType;
        public byte ClientIndex;
        public byte NegResult;
        public NatType NatType; //int
        public NatMappingScheme NatMappingScheme; //int
        public byte[] GameName = new byte[50];

        public new void Parse(byte[] recv)
        {
            base.Parse(recv);
            PortType = recv[13];
            ClientIndex = recv[14];
            NegResult = recv[15];

            byte[] tempNatType = ByteExtensions.SubBytes(recv, 17, sizeof(int));
            NatType = (NatType)BitConverter.ToInt32(tempNatType, 0);

            byte[] tempNatMappingScheme = ByteExtensions.SubBytes(recv, 19, sizeof(int));
            NatMappingScheme = (NatMappingScheme)BitConverter.ToInt32(tempNatMappingScheme, 0);

            Array.Copy(recv, 23, GameName, 0, 50);
        }

        public byte[] GenerateByteArray()
        {
            byte[] TempBytes = new byte[GetReplyPacketSize()];
            MagicData.CopyTo(TempBytes, 0);
            TempBytes[MagicData.Length] = Version;
            TempBytes[MagicData.Length + 1] = (byte)PacketType;
            BitConverter.GetBytes(Cookie).CopyTo(TempBytes, MagicData.Length + 2);

            TempBytes[BasePacket.Size] = PortType;
            TempBytes[BasePacket.Size + 1] = ClientIndex;
            TempBytes[BasePacket.Size + 2] = NegResult;
            BitConverter.GetBytes((int)NatType).CopyTo(TempBytes, BasePacket.Size + 3);
            BitConverter.GetBytes((int)NatMappingScheme).CopyTo(TempBytes, BasePacket.Size + 7);
            GameName.CopyTo(TempBytes, BasePacket.Size + 11);

            return TempBytes;
        }
    }
}
