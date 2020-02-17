using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NATNegotiation.Entity.Structure;
using System;
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
            Array.Copy(client.PublicIP, initPacket.LocalIP, 4);
            Array.Copy(client.PublicPort, initPacket.LocalPort, 2);
            byte[] buffer = initPacket.GenerateByteArray();
            server.SendAsync(client.EndPoint, buffer);
        }
    }
}
