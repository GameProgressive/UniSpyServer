using GameSpyLib.Network;
using NATNegotiation.Structure.Packet;

namespace NATNegotiation.Handler
{
    public class ConnectHandler
    {
        public static void ConnectResponse(NatNegServer server, UdpPacket packet)
        {
            ConnectPacket connectPacket = new ConnectPacket(packet.BytesRecieved);
           byte[] sendingBuffer= connectPacket.CreateReplyPacket();
            server.SendAsync(packet, sendingBuffer);
        }
    }
}
