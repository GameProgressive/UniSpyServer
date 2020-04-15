using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NatNegotiation.Entity.Structure;
using NatNegotiation.Handler.CommandHandler;
using GameSpyLib.Common.Entity.Interface;

namespace NatNegotiation.Handler.CommandHandler
{
    public class InitHandler : CommandHandlerBase
    {
        public InitHandler(IClient client, NatNegClientInfo clientInfo, byte[] recv) : base(client, clientInfo, recv)
        {
        }

        protected override void CheckRequest()
        {
            _initPacket = new InitPacket();
            _initPacket.Parse(_recv);
        }

        protected override void DataOperation()
        {
            _clientInfo.Parse(_initPacket);
            // client.GameName = ByteExtensions.SubBytes(recv, InitPacket.Size - 1, recv.Length - 1);
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = _initPacket.GenerateResponse(NatPacketType.InitAck);
        }
    }
}
