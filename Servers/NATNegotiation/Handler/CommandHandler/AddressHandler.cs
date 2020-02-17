using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Handler;
using System;
using System.Net;

namespace NatNegotiation.Handler.CommandHandler
{
    public class AddressHandler:NatNegHandlerBase
    {

        protected override void ConvertRequest(ClientInfo client, byte[] recv)
        {
           _initPacket = new InitPacket();
          _initPacket.Parse(recv);
        }

        protected override void ConstructResponsePacket(ClientInfo client, byte[] recv)
        {
            _initPacket.PacketType = (byte)NatPacketType.AddressReply;
            Array.Copy(client.PublicIP, _initPacket.LocalIP, 4);
            Array.Copy(client.PublicPort, _initPacket.LocalPort, 2);
            _sendingBuffer= _initPacket.GenerateByteArray();
        }
    }
}
