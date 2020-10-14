using GameSpyLib.Common.Entity.Interface;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NatNegotiation.Handler.SystemHandler.NatNegotiatorManage;

namespace NatNegotiation.Handler.CommandHandler
{
    public class InitHandler : NatNegCommandHandlerBase
    {
        protected InitPacket _initPacket;
        public InitHandler(ISession session, byte[] recv) : base(session, recv)
        {
        }

        protected override void CheckRequest()
        {
            _initPacket = new InitPacket();
            _initPacket.Parse(_recv);

            string key = _session.RemoteEndPoint.ToString() + "-" + _initPacket.PortType.ToString();

            if (!NatNegotiatorPool.Sessions.TryGetValue(key, out _))
            {
                NatNegotiatorPool.Sessions.TryAdd(key, _session);
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
            NatNegotiatorPool
                .Negotiate(
                _initPacket.PortType,
                _initPacket.Version,
                _initPacket.Cookie);
        }
    }
}
