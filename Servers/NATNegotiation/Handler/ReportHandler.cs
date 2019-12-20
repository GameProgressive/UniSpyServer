﻿using NATNegotiation.Structure.Packet;
using System.Net;

namespace NATNegotiation.Handler
{
    public class ReportHandler
    {
        public static void ReportResponse(NatNegServer server,byte[] recv)
        {
            ReportPacket reportPacket = new ReportPacket(recv);

            byte[] sendingBuffer = reportPacket.CreateReplyPacket();
            server.SendAsync(server.Socket.RemoteEndPoint, sendingBuffer);
        }
    }
}
