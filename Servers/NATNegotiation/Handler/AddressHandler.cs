using GameSpyLib.Network;
using NATNegotiation.Structure.Packet;

namespace NATNegotiation.Handler
{
    class AddressHandler
    {
        public static void AddressCheckResponse(NatNegServer server, UDPPacket packet)
        {
            InitPacket initPacket = new InitPacket(packet.BytesRecieved);
            byte[] sendingBuffer = initPacket.CreateReplyPacket();
            server.SendAsync(packet, sendingBuffer);
        }
    }
}
