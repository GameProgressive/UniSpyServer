using System;
using System.Collections.Generic;
using System.Net;
using NatNegotiation.Entity.Enumerator;

namespace NatNegotiation.Entity.Structure.Packet
{
    public class ConnectPacket : BasePacket
    {
        public EndPoint RemoteEndPoint { get; protected set; }
        public byte GotYourData { get; set; }
        public byte Finished { get; set; }

        public new static readonly int Size = BasePacket.Size + 8;

        public bool Parse(EndPoint endPoint,byte[] recv)
        {
            RemoteEndPoint = endPoint;
            return base.Parse(recv);
        }

        public override byte[] GenerateResponse(NatPacketType packetType)
        {
            List<byte> data = new List<byte>();
            data.AddRange(base.GenerateResponse(packetType));
            data.AddRange(((IPEndPoint)RemoteEndPoint).Address.GetAddressBytes());
            data.AddRange(BitConverter.GetBytes((short)((IPEndPoint)RemoteEndPoint).Port));
            data.Add(GotYourData);
            data.Add(Finished);

            return data.ToArray();
        }
    }
}
