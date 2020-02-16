using GameSpyLib.Encryption;
using NatNegotiation.Entity.Enumerator;
using System;

namespace NatNegotiation.Entity.Structure.Packet
{
    public class InitPacket:BasePacket
    {
        public static new readonly int Size = BasePacket.Size + 9;

        public byte PortType;
        public byte ClientIndex;
        public byte UseGamePort;
        public byte[] LocalIP = new byte[4];
        public byte[] LocalPort=new byte[2];

        public new void Parse(byte[] recv)
        {
            base.Parse(recv);
            PortType = recv[13];//02
            ClientIndex = recv[14];//00
            UseGamePort = recv[15];//00
            LocalIP = ByteExtensions.SubBytes(recv, 16, sizeof(uint));//00 - 00 - 00 - 00
            LocalPort = ByteExtensions.SubBytes(recv, 20, sizeof(ushort));//00 - 00
        }

        public byte[] GenerateByteArray()
        {
            byte[] TempBytes = new byte[GetReplyPacketSize()];
            MagicData.CopyTo(TempBytes, 0);
            TempBytes[MagicData.Length] = Version;
            TempBytes[MagicData.Length + 1] = (byte)NatPacketType.InitAck;
            Cookie.CopyTo(TempBytes, MagicData.Length + 2);

            TempBytes[BasePacket.Size] = PortType;
            TempBytes[BasePacket.Size + 1] = ClientIndex;
            TempBytes[BasePacket.Size + 2] = UseGamePort;
            LocalIP.CopyTo(TempBytes, BasePacket.Size + 3);
           LocalPort.CopyTo(TempBytes, BasePacket.Size + 7);
            return TempBytes;
        }

    }
}
