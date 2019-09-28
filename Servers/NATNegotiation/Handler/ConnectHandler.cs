using GameSpyLib.Network;
using GameSpyLib.Network.UDP;
using NATNegotiation.Structure.Packet;

namespace NATNegotiation.Handler
{
    public class ConnectHandler
    {
        public static void ConnectResponse(NatNegServer server, UDPPacket packet)
        {
            ConnectPacket connectPacket = new ConnectPacket(packet.BytesRecieved);
           byte[] sendingBuffer= connectPacket.CreateReplyPacket();
            server.SendAsync(packet, sendingBuffer);
        }
    }
}
