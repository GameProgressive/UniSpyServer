using GameSpyLib.Encryption;
using System;

namespace NatNegotiation.Entity.Structure.Packet
{
    public class InitPacket : BasePacket
    {
        public static new readonly int Size = BasePacket.Size + 9;

        public byte PortType;
        public byte ClientIndex;
        public byte UseGamePort;
        public byte[] LocalIP = new byte[4];
        public byte[] LocalPort = new byte[2];

        public new void Parse(byte[] recv)
        {
            base.Parse(recv);
            PortType = recv[BasePacket.Size];//
            ClientIndex = recv[BasePacket.Size + 1];//00
            UseGamePort = recv[BasePacket.Size + 2];//00
            Array.Copy(ByteTools.SubBytes(recv, BasePacket.Size + 3, sizeof(uint)), LocalIP, 4);
            Array.Copy(ByteTools.SubBytes(recv, BasePacket.Size + 7, sizeof(uint)), LocalPort, 2);
        }

        public byte[] GenerateByteArray()
        {
            byte[] TempBytes = new byte[Size];
            MagicData.CopyTo(TempBytes, 0);
            TempBytes[MagicData.Length] = Version;
            TempBytes[MagicData.Length + 1] = PacketType;
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
