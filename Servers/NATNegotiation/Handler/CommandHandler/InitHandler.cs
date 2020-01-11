using NATNegotiation.Entity.Structure.Packet;
using System.Net;

namespace NATNegotiation.Handler.CommandHandler
{
    public class InitHandler
    {
        public static void InitResponse(NatNegServer server, byte[] recv)
        {

            InitPacket initPacket = new InitPacket(recv);
            byte[] sendingBuffer = initPacket.CreateReplyPacket();
            server.SendAsync(server.Socket.RemoteEndPoint, sendingBuffer);
        }
    }
}
