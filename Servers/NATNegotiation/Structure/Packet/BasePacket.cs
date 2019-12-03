using GameSpyLib.Extensions;
using NATNegotiation.Enumerator;
using System;

namespace NATNegotiation.Structure.Packet
{
    public class BasePacket
    {
        public byte Version;
        public NatPacketType PacketType;
        public int Cookie;
        public int magicDataLen = NatNegInfo.MagicData.Length;
        public static readonly int BasePacketSize = NatNegInfo.MagicData.Length + 6;

        public BasePacket(byte[] data)
        {
            if (data.Length < BasePacketSize)
            {
                return;
            }

            byte[] sub = ByteExtensions.SubBytes(data, 0, magicDataLen);
            //bool isEqual = Array.Equals(sub, NatNegInfo.MagicData);
            if (sub[0] != NatNegInfo.MagicData[0] || sub[1] != NatNegInfo.MagicData[1] || sub[2] != NatNegInfo.MagicData[2])
                return;

            if (data[magicDataLen + 2] < 0 || data[magicDataLen + 2] > (byte)NatPacketType.PreInitAck)
                return;

            Version = data[magicDataLen];
            PacketType = (NatPacketType)data[magicDataLen + 1];
            Cookie = BitConverter.ToInt32(ByteExtensions.SubBytes(data, magicDataLen + 2, 4));
        }

        /// <summary>
        /// Get repsonse packet size from natneg recieved packet type
        /// </summary>
        /// <param name="type">recieved packet type</param>
        /// <returns></returns>
        protected int GetReplyPacketSize()
        {
            //The size is initially CommonInfo size
            int size = BasePacketSize;

            switch (PacketType)
            {
                case NatPacketType.PreInit:
                case NatPacketType.PreInitAck:
                    size += 6;
                    break;
                case NatPacketType.AddressCheck:
                case NatPacketType.NatifyRequest:
                case NatPacketType.ErtTest:
                case NatPacketType.ErtAck:
                case NatPacketType.Init:
                case NatPacketType.InitAck:
                case NatPacketType.ConnectAck:
                case NatPacketType.ReportAck:
                    size += 9;
                    break;

                case NatPacketType.ConnectPing:
                case NatPacketType.Connect:
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
