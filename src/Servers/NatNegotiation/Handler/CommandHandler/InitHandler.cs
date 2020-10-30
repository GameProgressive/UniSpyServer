using GameSpyLib.Abstraction.Interface;
using NatNegotiation.Abstraction.SystemHandler.NatNegotiationManage;
using NatNegotiation.Entity.Enumerate;
using NatNegotiation.Entity.Structure.Packet;

namespace NatNegotiation.Abstraction.BaseClass
{
    public class InitHandler : NatNegCommandHandlerBase
    {
        protected InitPacket _initPacket;
        public InitHandler(ISession session, byte[] recv) : base(session, recv)
        {
            _initPacket = new InitPacket();
        }

        protected override void CheckRequest()
        {
            _initPacket.Parse(_recv);

            string key = _session.RemoteEndPoint.ToString() + "-" + _initPacket.PortType.ToString();

            if (!NatNegotiationManager.SessionPool.TryGetValue(key, out _))
            {
                NatNegotiationManager.SessionPool.TryAdd(key, _session);
            }
        }

        protected override void DataOperation()
        {
            _session.UserInfo.Parse(_initPacket);
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = _initPacket
                .SetPacketType(NatPacketType.InitAck)
                .BuildResponse();
        }


        protected override void Response()
        {
            base.Response();
            NatNegotiationManager
                .Negotiate(
                _initPacket.PortType,
                _initPacket.Version,
                _initPacket.Cookie);
        }
    }
}
