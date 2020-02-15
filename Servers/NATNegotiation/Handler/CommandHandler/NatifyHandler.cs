using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NATNegotiation.Entity.Structure;
using System.Net;

namespace NatNegotiation.Handler.CommandHandler
{
    class NatifyHandler
    {
        public void Handle(NatNegServer server, ClientInfo client, byte[] recv)
        {
            InitPacket initPacket = new InitPacket();
            initPacket.Parse(recv);
            initPacket.PacketType = (byte)NatPacketType.ErtTest;
            if (initPacket.PortType == (byte)NatPortType.NN1)
            {
               byte[] buffer= initPacket.GenerateByteArray();
                server.SendAsync(client.EndPoint, buffer);
                return;
            }

        }
    }
}
