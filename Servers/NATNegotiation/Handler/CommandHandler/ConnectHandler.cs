using NATNegotiation.Entity.Structure.Packet;
using System.Net;

namespace NATNegotiation.Handler.CommandHandler
{
    public class ConnectHandler
    {
        public static void ConnectResponse(NatNegServer server, byte[] recv)
        {
            ConnectPacket connectPacket = new ConnectPacket(recv);
            byte[] sendingBuffer = connectPacket.CreateReplyPacket();
            server.SendAsync(server.Socket.RemoteEndPoint, sendingBuffer);
        }
    }
}
