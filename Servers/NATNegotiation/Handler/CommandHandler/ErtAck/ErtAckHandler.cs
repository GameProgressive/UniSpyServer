using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NatNegotiation.Entity.Structure;

namespace NatNegotiation.Handler.CommandHandler
{
    public class ErtACKHandler : CommandHandlerBase
    {
        protected override void CheckRequest(ClientInfo client, byte[] recv)
        {
            _initPacket = new InitPacket();
            _initPacket.Parse(recv);
        }

        protected override void DataOperation(ClientInfo client, byte[] recv)
        {
            client.Parse(_initPacket);
        }

        protected override void ConstructResponse(ClientInfo client, byte[] recv)
        {
            _sendingBuffer = _initPacket.GenerateResponse(NatPacketType.ErtAck, client.RemoteEndPoint);
        }
    }
}
