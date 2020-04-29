using GameSpyLib.Common.Entity.Interface;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure;
using NatNegotiation.Entity.Structure.Packet;

namespace NatNegotiation.Handler.CommandHandler
{
    public class NatifyHandler : NatNegCommandHandlerBase
    {
        public NatifyHandler(ISession session, byte[] recv) : base(session, recv)
        {
        }

        protected override void CheckRequest()
        {
            _initPacket = new InitPacket();
            _initPacket.Parse(_recv);
        }

        protected override void DataOperation()
        {
            _session.UserInfo.Version = _initPacket.Version;
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = _initPacket.GenerateResponse(NatPacketType.ErtTest, _session.RemoteEndPoint);
        }
    }
}
