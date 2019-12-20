using NATNegotiation.Structure.Packet;
using System.Net;

namespace NATNegotiation.Handler
{
    class AddressHandler
    {
        public static void AddressCheckResponse(NatNegServer server, byte[] recv)
        {
            InitPacket initPacket = new InitPacket(recv);
            byte[] sendingBuffer = initPacket.CreateReplyPacket();
            server.SendAsync(server.Socket.RemoteEndPoint, sendingBuffer);
        }
    }
}
