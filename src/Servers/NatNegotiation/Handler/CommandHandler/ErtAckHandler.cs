using GameSpyLib.Abstraction.Interface;
using NatNegotiation.Entity.Enumerate;
using NatNegotiation.Entity.Structure.Packet;

namespace NatNegotiation.Abstraction.BaseClass
{
    public class ErtACKHandler : NatNegCommandHandlerBase
    {
        protected InitPacket _initPacket;
        public ErtACKHandler(ISession session, byte[] recv) : base(session, recv)
        {
            _initPacket = new InitPacket();
        }

        protected override void CheckRequest()
        {
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
                .SetPacketType(NatPacketType.ErtAck)
                .BuildResponse();
        }
    }
}
