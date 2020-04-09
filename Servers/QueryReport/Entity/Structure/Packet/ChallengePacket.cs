using QueryReport.Entity.Enumerator;
using System;
using System.Collections.Generic;
using System.Net;

namespace QueryReport.Entity.Structure.Packet
{
    public class ChallengePacket : BasePacket
    {
        public int RemoteIP { get; protected set; }
        public int RemotePort { get; protected set; }

        public void Parse(EndPoint endPoint, byte[] recv)
        {
            base.Parse(recv);
            RemoteIP = BitConverter.ToInt32(((IPEndPoint)endPoint).Address.GetAddressBytes());
            RemotePort = ((IPEndPoint)endPoint).Port;
        }

        public override byte[] GenerateResponse()
        {
            //change packet type to challenge
            List<byte> data = new List<byte>();

            PacketType = QRPacketType.Challenge;

            data.AddRange(base.GenerateResponse());
            //Challenge
            data.AddRange(new byte[] { 0x54, 0x54, 0x54, 0x00, 0x00 });
            //IP
            data.AddRange(BitConverter.GetBytes(RemoteIP));
            data.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            //port
            data.AddRange(BitConverter.GetBytes(RemotePort));

            return data.ToArray();
        }
    }
}
