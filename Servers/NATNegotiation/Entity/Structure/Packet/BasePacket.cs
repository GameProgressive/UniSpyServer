using GameSpyLib.Encryption;
using NatNegotiation.Entity.Enumerator;
using NATNegotiation.Entity.Enumerator;
using System;
using System.Collections.Generic;

namespace NatNegotiation.Entity.Structure.Packet
{
    public class BasePacket
    {
        public static readonly byte[] MagicData = { 0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2 };
        public byte Version;
        public NatPacketType PacketType { get; set; }
        public int Cookie;

        public static readonly int Size = 12;

        public virtual bool Parse(byte[] recv)
        {
            if (recv.Length < Size)
            {
                return false;
            }

            Version = recv[MagicData.Length];
            PacketType = (NatPacketType)recv[MagicData.Length + 1];
            Cookie = BitConverter.ToInt32(ByteTools.SubBytes(recv, MagicData.Length + 2, 4));

            return true;
        }

        public virtual byte[] GenerateResponse(NatPacketType packetType)
        {
            List<byte> data = new List<byte>();
            data.AddRange(MagicData);
            data.Add(Version);
            data.Add((byte)packetType);
            data.AddRange(BitConverter.GetBytes(Cookie));

            return data.ToArray();
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

            switch (PacketType)
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
