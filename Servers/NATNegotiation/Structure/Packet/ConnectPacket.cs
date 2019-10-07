using GameSpyLib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NATNegotiation.Structure.Packet
{
    public class ConnectPacket : BasePacket
    {
        public uint RemoteIP;
        public ushort RemotePort;
        public byte GotYourData;
        public byte Finished;

        //private static int ConnectPacketSize = 8;
        //public static readonly int PacketSize = BasePacketSize + ConnectPacketSize;
        public ConnectPacket(byte[] data) : base(data)
        {
            RemoteIP = BitConverter.ToUInt32(ByteExtensions.SubBytes(data, 13, sizeof(uint)));
            RemotePort = BitConverter.ToUInt16(ByteExtensions.SubBytes(data, 17, sizeof(ushort)));
            GotYourData = data[19];
            Finished = data[20];
        }

        public byte[] CreateReplyPacket()
        {
            byte[] TempBytes = new byte[GetReplyPacketSize()];
            NatNegInfo.MagicData.CopyTo(TempBytes, 0);
            TempBytes[magicDataLen] = Version;
            TempBytes[magicDataLen + 1] = (byte)PacketType;
            BitConverter.GetBytes(Cookie).CopyTo(TempBytes, magicDataLen + 2);

            // Cache the client info on the init packet and then access them with the cookie and send GotConnectAck to true
            BitConverter.GetBytes(RemoteIP).CopyTo(TempBytes, BasePacketSize);
            BitConverter.GetBytes(RemotePort).CopyTo(TempBytes, BasePacketSize + 4);

            TempBytes[BasePacketSize+5] = GotYourData;
            TempBytes[BasePacketSize + 6] = Finished;
            return TempBytes;

        }
        }
}
