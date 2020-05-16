using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Extensions;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure;
using NatNegotiation.Entity.Structure.Packet;

namespace NatNegotiation.Handler.CommandHandler
{
    public class ErtACKHandler : NatNegCommandHandlerBase
    {
        public ErtACKHandler(ISession session, byte[] recv) : base(session, recv)
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
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer =
                _initPacket.SetIPAndPortForResponse(_session.RemoteEndPoint)
                .BuildResponse(NatPacketType.ErtAck);
        }
    }
}
