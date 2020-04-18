using GameSpyLib.Common.Entity.Interface;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure;
using NatNegotiation.Entity.Structure.Packet;

namespace NatNegotiation.Handler.CommandHandler
{
    public class AddressHandler : NatNegCommandHandlerBase
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
