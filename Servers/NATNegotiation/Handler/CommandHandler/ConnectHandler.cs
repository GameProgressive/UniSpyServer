using NatNegotiation.Entity.Structure.Packet;
using System.Net;

namespace NatNegotiation.Handler.CommandHandler
{
    public class ConnectHandler
    {
        public void Handle(NatNegServer server, EndPoint endPoint, byte[] recv)
        {
            //ConnectPacket connectPacket = new ConnectPacket(recv);
            //byte[] sendingBuffer = connectPacket.CreateReplyPacket();
            //server.SendAsync(server.Socket.RemoteEndPoint, sendingBuffer);
        }
    }
}
