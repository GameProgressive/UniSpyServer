using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NatNegotiation.Entity.Structure;
using GameSpyLib.Common.Entity.Interface;

namespace NatNegotiation.Handler.CommandHandler
{
    public class ErtACKHandler : CommandHandlerBase
    {
        public ErtACKHandler(IClient client, NatNegClientInfo clientInfo, byte[] recv) : base(client, clientInfo, recv)
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
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = _initPacket.GenerateResponse(NatPacketType.ErtAck, _clientInfo.RemoteEndPoint);
        }
    }
}
