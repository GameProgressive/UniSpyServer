using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NatNegotiation.Entity.Structure;
using NatNegotiation.Handler.CommandHandler;
using GameSpyLib.Common.Entity.Interface;

namespace NatNegotiation.Handler.CommandHandler
{
    public class NatifyHandler : NatNegCommandHandlerBase
    {
        public NatifyHandler(IClient client, NatNegClientInfo clientInfo, byte[] recv) : base(client, clientInfo, recv)
        {
        }

        protected override void CheckRequest()
        {
            _initPacket = new InitPacket();
            _initPacket.Parse(_recv);
        }

        protected override void DataOperation()
        {
            _clientInfo.Version = _initPacket.Version;
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = _initPacket.GenerateResponse(NatPacketType.ErtTest, _clientInfo.RemoteEndPoint);
        }
    }
}
