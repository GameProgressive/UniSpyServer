using GameSpyLib.Encryption;
using NatNegotiation.Entity.Enumerator;
using NATNegotiation.Entity.Enumerator;
using System;

namespace NatNegotiation.Entity.Structure.Packet
{
    public class BasePacket
    {
        public static readonly byte[] MagicData = { 0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2 };
        public byte Version;
        public byte PacketType;
        public byte[] Cookie = new byte[4];

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
            //if (!ByteExtensions.SubBytes(recv, 0, MagicData.Length).Equals(MagicData))
            //{
            //    ErrorCode = NNErrorCode.MagicDataError;
            //    return;
            //}
            //check packet type
            //if (recv[MagicData.Length + 2] < 0 || recv[MagicData.Length + 2] > (byte)NatPacketType.PreInitAck)
            //{
            //    ErrorCode = NNErrorCode.PacketTypeError;
            //    return;
            //}

            Version = recv[MagicData.Length];
            PacketType = recv[MagicData.Length + 1];
            Array.Copy(ByteExtensions.SubBytes(recv, MagicData.Length + 2, 4), Cookie, 4);
        }


        /// <summary>
        /// Get repsonse packet size from natneg recieved packet type
        /// </summary>
        /// <param name="type">recieved packet type</param>
        /// <returns></returns>
        public int GetReplyPacketSize()
        {
            //The size is initially CommonInfo size
            int size = BasePacket.Size;

            switch ((NatPacketType)PacketType)
            {
                case NatPacketType.PreInit:
                case NatPacketType.PreInitAck:
                    size += 6;
                    break;
                case NatPacketType.AddressCheck:
                case NatPacketType.AddressReply:
                case NatPacketType.NatifyRequest:
                case NatPacketType.ErtTest:
                case NatPacketType.ErtAck:
                case NatPacketType.Init:
                case NatPacketType.InitAck:
                case NatPacketType.ConnectAck:
                case NatPacketType.ReportAck:
                    size += 9;
                    break;
                case NatPacketType.Connect:
                case NatPacketType.ConnectPing:
                    size += 8;
                    break;
                case NatPacketType.Report:
                    size += 61;
                    break;
                default:
                    break;
            }
            return size;
        }
    }
}
