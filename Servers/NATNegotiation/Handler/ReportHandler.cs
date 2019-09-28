using GameSpyLib.Network.UDP;
using NATNegotiation.Structure.Packet;

namespace NATNegotiation.Handler
{
    public class ReportHandler
    {
        public static void ReportResponse(NatNegServer server, UDPPacket packet)
        {
            ReportPacket reportPacket = new ReportPacket(packet.BytesRecieved);
           
            byte[] sendingBuffer = reportPacket.CreateReplyPacket();
            server.Send(packet, sendingBuffer);
        }
    }
}
