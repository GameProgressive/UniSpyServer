using GameSpyLib.Network;
using NATNegotiation.Structure.Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace NATNegotiation.Handler
{
    public class ReportHandler
    {
        public static void ReportResponse(NatNegServer server, UdpPacket packet)
        {
            ReportPacket reportPacket = new ReportPacket(packet.BytesRecieved);
           
            byte[] sendingBuffer = reportPacket.CreateReplyPacket();
            server.SendAsync(packet, sendingBuffer);
        }
    }
}
