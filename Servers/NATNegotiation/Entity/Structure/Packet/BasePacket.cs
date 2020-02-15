using GameSpyLib.Encryption;
using GameSpyLib.Logging;
using NatNegotiation.Entity.Enumerator;
using NATNegotiation.Entity.Enumerator;
using NATNegotiation.Entity.Structure;
using System;

namespace NatNegotiation.Entity.Structure.Packet
{
    public class BasePacket
    {
        public static readonly byte[] MagicData = { 0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2 };
        public byte Version;
        public NatPacketType PacketType;
        public int Cookie;
        public static readonly int Size = 12;
        public NNErrorCode ErrorCode { get; protected set; }

        public void Parse(byte[] recv)
        {
            ErrorCode = NNErrorCode.NoError;
            if (recv.Length < Size)
            {
                ErrorCode = NNErrorCode.RequestError;
                return;
            }
            //check magic data
            if (!ByteExtensions.SubBytes(recv, 0, MagicData.Length).Equals(MagicData))
            {
                ErrorCode = NNErrorCode.MagicDataError;
                return;
            }
            //check packet type
            if (recv[MagicData.Length + 2] < 0 || recv[MagicData.Length + 2] > (byte)NatPacketType.PreInitAck)
            {
                ErrorCode = NNErrorCode.PacketTypeError;
                return;
            }

            Version = recv[MagicData.Length];
            PacketType = (NatPacketType)recv[MagicData.Length + 1];
            Cookie = BitConverter.ToInt32(ByteExtensions.SubBytes(recv, MagicData.Length + 2, 4));
        }

    }
}
