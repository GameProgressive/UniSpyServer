using NatNegotiation.Entity.Structure.Packet;
using NATNegotiation.Entity.Structure;
using System;
using System.Net;

namespace NatNegotiation.Handler.CommandHandler
{
    public class AddressHandler
    {
        public void Handle(NatNegServer server,ClientInfo client, byte[] recv)
        {
            InitPacket initPacket = new InitPacket();
            initPacket.Parse(recv);
            initPacket.LocalIp = ((IPEndPoint)client.EndPoint).Address.GetAddressBytes();
            initPacket.LocalPort = BitConverter.GetBytes(((IPEndPoint)client.EndPoint).Port);
            byte[] buffer = initPacket.GenerateByteArray();
            server.SendAsync(client.EndPoint, buffer);
        }
    }
}
