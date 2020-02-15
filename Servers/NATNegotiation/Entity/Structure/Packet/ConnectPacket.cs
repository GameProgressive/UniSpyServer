using GameSpyLib.Encryption;
using NatNegotiation.Entity.Enumerator;
using NATNegotiation.Entity.Structure;
using System;
using System.Net;

namespace NatNegotiation.Entity.Structure.Packet
{
    public class ConnectPacket:BasePacket
    {
        public uint RemoteIP;
        public ushort RemotePort;
        public byte GotYourData;
        public byte Finished;

        public new static readonly int Size = BasePacket.Size + 8;


        public new void Parse(byte[] recv)
        {
            base.Parse(recv);
        }

        public byte[] GenerateByteArray()
        {
            byte[] TempBytes = new byte[GetReplyPacketSize()];
            MagicData.CopyTo(TempBytes, 0);
            TempBytes[MagicData.Length] = Version;
            TempBytes[MagicData.Length + 1] = (byte)PacketType;
            BitConverter.GetBytes(Cookie).CopyTo(TempBytes, MagicData.Length + 2);

            // Cache the client info on the init packet and then access them with the cookie and send GotConnectAck to true
            BitConverter.GetBytes(RemoteIP).CopyTo(TempBytes, BasePacket.Size);
            BitConverter.GetBytes(RemotePort).CopyTo(TempBytes, BasePacket.Size + 4);

            TempBytes[BasePacket.Size + 5] = GotYourData;
            TempBytes[BasePacket.Size + 6] = Finished;
            return TempBytes;

        }
        
    }
}
