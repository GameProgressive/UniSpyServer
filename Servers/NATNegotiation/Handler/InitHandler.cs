using NATNegotiation.Structure.Packet;
using System.Net;

namespace NATNegotiation.Handler
{
    public class InitHandler
    {
        public static void InitResponse(NatNegServer server, EndPoint endpoint, byte[] recv)
        {

            InitPacket initPacket = new InitPacket(recv);
            byte[] sendingBuffer = initPacket.CreateReplyPacket();
            server.SendAsync(endpoint, sendingBuffer);
        }
    }
}
