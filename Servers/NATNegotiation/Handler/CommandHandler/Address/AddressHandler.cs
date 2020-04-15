using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NatNegotiation.Entity.Structure;
using NatNegotiation.Handler.CommandHandler;
using GameSpyLib.Common.Entity.Interface;

namespace NatNegotiation.Handler.CommandHandler
{
    public class AddressHandler : CommandHandlerBase
    {
        public AddressHandler(IClient client, NatNegClientInfo clientInfo, byte[] recv) : base(client, clientInfo, recv)
        {
        }

        protected override void CheckRequest()
        {
            _initPacket = new InitPacket();
            _initPacket.Parse(_recv);
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = _initPacket.GenerateResponse(NatPacketType.AddressReply, _clientInfo.RemoteEndPoint);
        }
    }
}
