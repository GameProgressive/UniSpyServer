using GameSpyLib.Common.Entity.Interface;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure;
using NatNegotiation.Entity.Structure.Packet;

namespace NatNegotiation.Handler.CommandHandler
{
    public class ErtACKHandler : NatNegCommandHandlerBase
    {
        public ErtACKHandler(ISession session, NatNegUserInfo clientInfo, byte[] recv) : base(session, clientInfo, recv)
        {
        }

        protected override void CheckRequest()
        {
            _initPacket = new InitPacket();
            _initPacket.Parse(_recv);
        }

        protected override void DataOperation()
        {
            _userInfo.Parse(_initPacket);
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = _initPacket.GenerateResponse(NatPacketType.ErtAck, _userInfo.RemoteEndPoint);
        }
    }
}
