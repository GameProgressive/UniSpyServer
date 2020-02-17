using System;
using NatNegotiation;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NATNegotiation.Entity.Structure;

namespace NATNegotiation.Handler.CommandHandler
{
    public class ErtACKHandler : NatNegHandlerBase
    {
        protected override void ConvertRequest(ClientInfo client, byte[] recv)
        {
            _initPacket = new InitPacket();
            _initPacket.Parse(recv);
        }
        protected override void ProcessInformation(ClientInfo client, byte[] recv)
        {
            client.IsGotErtAck = true;
        }
        protected override void ConstructResponsePacket(ClientInfo client, byte[] recv)
        {
            Array.Copy(client.PublicIP, _initPacket.LocalIP, 4);
            Array.Copy(client.PublicPort, _initPacket.LocalPort, 2);
            _sendingBuffer = _initPacket.GenerateByteArray();
        }


    }
}
