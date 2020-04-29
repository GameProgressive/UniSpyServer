using GameSpyLib.Common.Entity.Interface;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;

namespace NatNegotiation.Handler.CommandHandler
{
    public class InitHandler : NatNegCommandHandlerBase
    {
        public InitHandler(ISession session, byte[] recv) : base(session, recv)
        {
        }

        protected override void CheckRequest()
        {
            _initPacket = new InitPacket();
            _initPacket.Parse(_recv);
        }

        protected override void DataOperation()
        {
            _session.UserInfo.Parse(_initPacket);
            // client.GameName = ByteExtensions.SubBytes(recv, InitPacket.Size - 1, recv.Length - 1);
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = _initPacket.GenerateResponse(NatPacketType.InitAck);
        }
    }
}
