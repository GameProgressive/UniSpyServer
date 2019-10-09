using NATNegotiation.Structure.Packet;
using System.Net;

namespace NATNegotiation.Handler
{
    class NatifyHandler
    {
        public static void NatifyResponse(NatNegServer server, EndPoint endpoint, byte[] recv)
        {
            InitPacket initPacket = new InitPacket(recv);
            byte[] sendingBuffer = initPacket.CreateReplyPacket();
            server.SendAsync(endpoint, sendingBuffer);
        }
    }
}
