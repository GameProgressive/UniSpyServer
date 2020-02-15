using NatNegotiation.Entity.Structure.Packet;
using NATNegotiation.Entity.Structure;
using System.Net;

namespace NatNegotiation.Handler.CommandHandler
{
    public class ReportHandler
    {
        public void Handle(NatNegServer server, ClientInfo client, byte[] recv)
        {
            ReportPacket reportPacket = new ReportPacket();
            reportPacket.Parse(recv);
            reportPacket.PacketType = Entity.Enumerator.NatPacketType.ReportAck;
            byte[] buffer = reportPacket.GenerateByteArray();
            server.SendAsync(client.EndPoint, buffer);
        }
    }
}
