using System.Collections.Generic;
using System.Linq;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NatNegotiation.Handler.SystemHandler.NegotiationSession;
using NatNegotiation.Server;
using Serilog.Events;

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
            SendConnectPacketToEachother();
        }


        private void SendConnectPacketToEachother()
        {
            List<NatNegSession> result = NatNegotiatorPool.Sessions.
                Where(s => s.Key.Contains(_initPacket.PortType.ToString())).
                Select(s => s.Value).ToList();

            if (result.Count < 2)
            {
                LogWriter.ToLog(LogEventLevel.Debug, "No match found we contine wait.");
                return;
            }

            var negotiatorPairs = result.Where(s => s.UserInfo.Cookie == _initPacket.Cookie);

            var negotiators = negotiatorPairs.Where(s => s.UserInfo.ClientIndex == 0);
       

            var negotiatees = negotiatorPairs.Where(s => s.UserInfo.ClientIndex == 1);

            if (negotiators.Count() < 1|| negotiatees.Count() < 1)
            {
                return;
            }
            var negotiator = negotiators.First();
            var negotiatee = negotiatees.First();

            LogWriter.ToLog(LogEventLevel.Debug, $"Find negotiator {negotiator.RemoteEndPoint}");
            LogWriter.ToLog(LogEventLevel.Debug, $"Find negotiatee {negotiatee.RemoteEndPoint}");

            ConnectPacket packetToNegotiator = new ConnectPacket();
            byte[] dataToNegotiator = packetToNegotiator.Parse(_initPacket).
                    SetRemoteEndPoint(negotiatee.RemoteEndPoint).
                    BuildResponse();

            ConnectPacket packetToNegotiatee = new ConnectPacket();
            byte[] dataToNegotiatee = packetToNegotiatee.Parse(_initPacket).
                SetRemoteEndPoint(negotiator.RemoteEndPoint).
                BuildResponse();


            negotiator.SendAsync(dataToNegotiator);
            negotiatee.SendAsync(dataToNegotiatee);

        }
    }
}
