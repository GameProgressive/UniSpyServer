using GameSpyLib.Extensions;
using NatNegotiation.Entity.Enumerator;
using System;
using System.Collections.Generic;
using System.Net;

namespace NatNegotiation.Entity.Structure.Packet
{
    public class InitPacket : BasePacket
    {
        public static new readonly int Size = BasePacket.Size + 9;

        public byte PortType { get; protected set; }
        public byte ClientIndex { get; protected set; }
        public byte UseGamePort { get; protected set; }
        public int LocalIP;
        public short LocalPort;

        public override bool Parse(byte[] recv)
        {
            if (!base.Parse(recv))
            {
                return false;
            }
            PortType = recv[BasePacket.Size];//
            ClientIndex = recv[BasePacket.Size + 1];//00
            UseGamePort = recv[BasePacket.Size + 2];//00
            LocalIP = BitConverter.ToInt32(ByteTools.SubBytes(recv, BasePacket.Size + 3, 4));
            LocalPort = BitConverter.ToInt16(ByteTools.SubBytes(recv, BasePacket.Size + 7, 2));
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packetType"></param>
        /// <param name="endPoint">public endpoint</param>
        /// <returns></returns>
        public byte[] GenerateResponse(NatPacketType packetType, EndPoint endPoint)
        {
            List<byte> data = new List<byte>();

            data.AddRange(base.GenerateResponse(packetType));

            data.Add(PortType);
            data.Add(ClientIndex);
            data.Add(UseGamePort);

            data.AddRange(((IPEndPoint)endPoint).Address.GetAddressBytes());
            data.AddRange(BitConverter.GetBytes((short)((IPEndPoint)endPoint).Port));

            return data.ToArray();
        }

        public override byte[] GenerateResponse(NatPacketType packetType)
        {

            List<byte> data = new List<byte>();

            data.AddRange(base.GenerateResponse(packetType));

            data.Add(PortType);
            data.Add(ClientIndex);
            data.Add(UseGamePort);
            data.AddRange(BitConverter.GetBytes(LocalIP));
            data.AddRange(BitConverter.GetBytes(LocalPort));

            return data.ToArray();
        }
    }
}
