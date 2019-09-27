using GameSpyLib.Network;
using NATNegotiation.Structure.Packet;

namespace NATNegotiation.Handler
{
    public class InitHandler
    {
        public static void InitResponse(NatNegServer server, UDPPacket packet)
        {
            InitPacket initPacket = new InitPacket(packet.BytesRecieved);
            byte[] sendingBuffer = initPacket.CreateReplyPacket();
            server.SendAsync(packet, sendingBuffer);
        }
    }
}
