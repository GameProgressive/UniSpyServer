using NatNegotiation.Entity.Structure.Packet;
using System.Net;

namespace NatNegotiation.Handler.CommandHandler
{
    public class ReportHandler
    {
        public void Handle(NatNegServer server, EndPoint endPoint, byte[] recv)
        {
            ReportPacket reportPacket = new ReportPacket(recv);
            byte[] sendingBuffer = reportPacket.CreateReplyPacket();
            server.SendAsync(server.Socket.RemoteEndPoint, sendingBuffer);
        }
    }
}
