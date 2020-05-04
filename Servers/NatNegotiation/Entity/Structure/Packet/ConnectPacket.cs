using NatNegotiation.Entity.Enumerator;
using System;
using System.Collections.Generic;
using System.Net;

namespace NatNegotiation.Entity.Structure.Packet
{
    public class ConnectPacket : BasePacket
    {
        public EndPoint RemoteEndPoint { get; protected set; }
        public byte GotYourData { get; set; }
        public byte Finished { get; set; }

        public bool Parse(EndPoint endPoint, byte[] recv)
        {
            if (!base.Parse(recv))
            {
                return false;
            }
            RemoteEndPoint = endPoint;
            return true;
        }

        public override byte[] BuildResponse(NatPacketType packetType)
        {
            List<byte> data = new List<byte>();
            data.AddRange(base.BuildResponse(packetType));
            data.AddRange(((IPEndPoint)RemoteEndPoint).Address.GetAddressBytes());
            data.AddRange(BitConverter.GetBytes((short)((IPEndPoint)RemoteEndPoint).Port));
            data.Add(GotYourData);
            data.Add(Finished);

            return data.ToArray();
        }
    }
}
