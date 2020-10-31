using System;
using System.Collections.Generic;
using GameSpyLib.Extensions;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;
using QueryReport.Entity.Structure.Request;

namespace QueryReport.Entity.Structure.Response
{
    public class ChallengeResponse : QRResponseBase
    {
        public string RemoteIP { get; protected set; }
        public string RemotePort { get; protected set; }
        public static readonly byte[] Challenge= { 0x54, 0x54, 0x54, 0x00, 0x00 };
        public static readonly byte[] Spliter = { 0x00, 0x00, 0x00, 0x00 };

        public ChallengeResponse(ChallengeRequest request) : base(request)
        {
            RemoteIP = request.RemoteIP;
            RemotePort = request.RemotePort;
            PacketType = QRPacketType.Challenge;
        }

        public override byte[] BuildResponse()
        {
            //change packet type to challenge
            List<byte> data = new List<byte>();

            data.AddRange(base.BuildResponse());
            //Challenge
            data.AddRange(Challenge);
            //IP
            data.AddRange(HtonsExtensions.IPStringToBytes(RemoteIP));
            data.AddRange(Spliter);
            //port
            data.AddRange(HtonsExtensions.PortToIntBytes(RemotePort));

            return data.ToArray();
        }
    }
}
