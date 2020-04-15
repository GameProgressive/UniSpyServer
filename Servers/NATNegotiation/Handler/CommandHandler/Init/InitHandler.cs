using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NatNegotiation.Entity.Structure;
using NatNegotiation.Handler.CommandHandler;

namespace NatNegotiation.Handler.CommandHandler
{
    public class InitHandler : CommandHandlerBase
    {
        protected override void ConvertRequest(ClientInfo client, byte[] recv)
        {
            _initPacket = new InitPacket();
            _initPacket.Parse(recv);
        }

        protected override void ProcessInformation(ClientInfo client, byte[] recv)
        {
            client.Parse(_initPacket);
            // client.GameName = ByteExtensions.SubBytes(recv, InitPacket.Size - 1, recv.Length - 1);
        }

        protected override void ConstructResponsePacket(ClientInfo client, byte[] recv)
        {
            _sendingBuffer = _initPacket.GenerateResponse(NatPacketType.InitAck);
        }
    }
}
