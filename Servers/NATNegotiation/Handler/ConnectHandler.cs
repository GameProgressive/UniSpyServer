using NATNegotiation.Structure.Packet;
using System.Net;

namespace NATNegotiation.Handler
{
    public class ConnectHandler
    {
        public static void ConnectResponse(NatNegServer server, EndPoint endpoint, byte[] recv)
        {
            ConnectPacket connectPacket = new ConnectPacket(recv);
            byte[] sendingBuffer = connectPacket.CreateReplyPacket();
            server.SendAsync(endpoint, sendingBuffer);
        }
    }
}
