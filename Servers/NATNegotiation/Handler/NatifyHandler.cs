using GameSpyLib.Network;
using NATNegotiation.Structure.Packet;

namespace NATNegotiation.Handler
{
    class NatifyHandler
    {
        public static void NatifyResponse(NatNegServer server, UDPPacket packet)
        {
            InitPacket initPacket = new InitPacket(packet.BytesRecieved);
            byte[] sendingBuffer = initPacket.CreateReplyPacket();
            server.SendAsync(packet, sendingBuffer);
        }
    }
}
