using System;
using System.Collections.Generic;
using GameSpyLib.Extensions;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;
using QueryReport.Entity.Structure.Request;
using QueryReport.Server;

namespace QueryReport.Entity.Structure.Response
{
    public class HeartBeatResponse : QRResponseBase
    {
        public string RemoteIP { get; protected set; }
        public string RemotePort { get; protected set; }
        public static readonly byte[] Challenge= { 0x54, 0x54, 0x54, 0x00, 0x00 };
        public static readonly byte[] Spliter = { 0x00, 0x00, 0x00, 0x00 };

        public HeartBeatResponse(QRSession session,HeartBeatRequest request) : base(request)
        {
            RemoteIP = HtonsExtensions.EndPointToIP(session.RemoteEndPoint);
            RemotePort = HtonsExtensions.EndPointToPort(session.RemoteEndPoint);
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
