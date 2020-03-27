using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NATNegotiation.Entity.Structure;
using System;
using System.Net;

namespace NATNegotiation.Handler.CommandHandler
{
    public class ErtACKHandler : CommandHandlerBase
    {
        protected override void ConvertRequest(ClientInfo client, byte[] recv)
        {
            _initPacket = new InitPacket();
            _initPacket.Parse(recv);
        }

        protected override void ProcessInformation(ClientInfo client, byte[] recv)
        {
            client.Parse(_initPacket);
        }

        protected override void ConstructResponsePacket(ClientInfo client, byte[] recv)
        {
            _sendingBuffer = _initPacket.GenerateResponse(NatPacketType.ErtAck,client.RemoteEndPoint);
        }
    }
}
