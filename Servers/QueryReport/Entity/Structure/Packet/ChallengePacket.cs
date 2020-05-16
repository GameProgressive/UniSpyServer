using GameSpyLib.Extensions;
using QueryReport.Entity.Enumerator;
using System;
using System.Collections.Generic;
using System.Net;

namespace QueryReport.Entity.Structure.Packet
{
    public class ChallengePacket : BasePacket
    {
        public string RemoteIP { get; protected set; }
        public string RemotePort { get; protected set; }

        public void Parse(EndPoint endPoint, byte[] recv)
        {
            base.Parse(recv);
            RemoteIP = HtonsExtensions.EndPointToIPString(endPoint);
            RemotePort = HtonsExtensions.EndPointToPortString(endPoint);
        }

        public override byte[] BuildResponse()
        {
            //change packet type to challenge
            List<byte> data = new List<byte>();

            PacketType = QRPacketType.Challenge;

            data.AddRange(base.BuildResponse());
            //Challenge
            data.AddRange(new byte[] { 0x54, 0x54, 0x54, 0x00, 0x00 });
            //IP
            data.AddRange(HtonsExtensions.IPStringToBytes(RemoteIP));
            data.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            //port
            data.AddRange(HtonsExtensions.PortToIntBytes(RemotePort));

            return data.ToArray();
        }
    }
}
