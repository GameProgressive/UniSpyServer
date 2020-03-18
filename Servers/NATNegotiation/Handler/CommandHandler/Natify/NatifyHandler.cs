using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Handler.CommandHandler;
using NATNegotiation.Handler.SystemHandler;
using System;

namespace NatNegotiation.Handler.CommandHandler
{
    public class NatifyHandler : CommandHandlerBase
    {
        protected override void ConvertRequest(ClientInfo client, byte[] recv)
        {
            _initPacket = new InitPacket();
            _initPacket.Parse(recv);
        }

        protected override void ProcessInformation(ClientInfo client, byte[] recv)
        {
            client.Version = _initPacket.Version;
            Array.Copy(NNFormat.IPToByte(client.RemoteEndPoint), client.PublicIP, 4);
            Array.Copy(NNFormat.PortToByte(client.RemoteEndPoint), client.PublicPort, 2);
        }

        protected override void ConstructResponsePacket(ClientInfo client, byte[] recv)
        {
            _initPacket.PacketType = (byte)NatPacketType.ErtTest;
            Array.Copy(client.PublicIP, _initPacket.LocalIP, 4);
            Array.Copy(client.PublicPort, _initPacket.LocalPort, 2);
            _sendingBuffer = _initPacket.GenerateByteArray();
        }

    }
}
