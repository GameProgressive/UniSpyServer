using GameSpyLib.Encryption;
using NATNegotiation.Entity.Structure;
using System;
using System.Net;

namespace NatNegotiation.Entity.Structure.Packet
{
    public class ConnectPacket:BasePacket
    {
        public uint RemoteIP;
        public uint RemotePort;
        public byte GotYourData;
        public byte Finished;

        public new static readonly int Size = BasePacket.Size + 8;


        public new void Parse(byte[] recv)
        {
            base.Parse(recv);
        
        }

        //public byte[] CreateReplyPacket()
        //{
        //    byte[] TempBytes = new byte[BasePacket.GetReplyPacketSize()];
        //    NatNegInfo.MagicData.CopyTo(TempBytes, 0);
        //    TempBytes[magicDataLen] = Version;
        //    TempBytes[magicDataLen + 1] = (byte)PacketType;
        //    BitConverter.GetBytes(Cookie).CopyTo(TempBytes, magicDataLen + 2);

        //    // Cache the client info on the init packet and then access them with the cookie and send GotConnectAck to true
        //    BitConverter.GetBytes(RemoteIP).CopyTo(TempBytes, BasePacketSize);
        //    BitConverter.GetBytes(RemotePort).CopyTo(TempBytes, BasePacketSize + 4);

        //    TempBytes[BasePacketSize + 5] = GotYourData;
        //    TempBytes[BasePacketSize + 6] = Finished;
        //    return TempBytes;

        //}
    }
}
