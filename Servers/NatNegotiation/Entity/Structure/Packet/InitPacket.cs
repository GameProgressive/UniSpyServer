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
        public string LocalIP;
        public ushort LocalPort;
        private EndPoint _endPoint;
        public override bool Parse(byte[] recv)
        {
            if (!base.Parse(recv))
            {
                return false;
            }
            PortType = recv[BasePacket.Size];//
            ClientIndex = recv[BasePacket.Size + 1];//00
            UseGamePort = recv[BasePacket.Size + 2];//00

            LocalIP = HtonsExtensions.
                BytesToIPString(
                ByteTools.SubBytes(recv, BasePacket.Size + 3, 4));

            LocalPort = HtonsExtensions.
                BytesToUshortPort(
                ByteTools.SubBytes(recv, BasePacket.Size + 7, 2));

            return true;
        }

        public override byte[] BuildResponse(NatPacketType packetType)
        {

            List<byte> data = new List<byte>();

            data.AddRange(base.BuildResponse(packetType));

            data.Add(PortType);
            data.Add(ClientIndex);
            data.Add(UseGamePort);

            if (_endPoint == null)
            {
                data.AddRange(HtonsExtensions.IPStringToBytes(LocalIP));
                data.AddRange(HtonsExtensions.PortToUshortBytes(LocalPort));
            }
            else
            {
                data.AddRange(((IPEndPoint)_endPoint).Address.GetAddressBytes());
                data.AddRange(BitConverter.GetBytes((short)((IPEndPoint)_endPoint).Port));
            }

            return data.ToArray();
        }

        public InitPacket SetEndPoint(EndPoint end)
        {
            _endPoint = end;
            return this;
        }
    }
}
