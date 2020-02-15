using NatNegotiation.Entity.Structure.Packet;
using NATNegotiation.Entity.Structure;
using System.Net;

namespace NatNegotiation.Handler.CommandHandler
{
    public class ReportHandler
    {
        public void Handle(NatNegServer server, ClientInfo client, byte[] recv)
        {
            //ReportPacket reportPacket = new ReportPacket(recv);
            //byte[] sendingBuffer = reportPacket.CreateReplyPacket();
            //server.SendAsync(server.Socket.RemoteEndPoint, sendingBuffer);
        }
    }
}
