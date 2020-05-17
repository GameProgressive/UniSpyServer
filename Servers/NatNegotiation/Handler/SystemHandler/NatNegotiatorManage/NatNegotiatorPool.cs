using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GameSpyLib.Logging;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NatNegotiation.Server;
using Serilog.Events;

namespace NatNegotiation.Handler.SystemHandler.NatNegotiatorManage
{
    public class NatNegotiatorPool
    {
        public static ConcurrentDictionary<string, NatNegSession> Sessions;

        public NatNegotiatorPool()
        {
            Sessions = new ConcurrentDictionary<string, NatNegSession>();
        }

        public void Start()
        { }

        public static void FindNatNegotiatorsAndSendConnectPacket(NatPortType portType, byte version, uint cookie)
        {
            List<NatNegSession> result = Sessions.
                        Where(s => s.Key.Contains(portType.ToString())).
                        Select(s => s.Value).ToList();

            if (result.Count < 2)
            {
                LogWriter.ToLog(LogEventLevel.Debug, "No match found we contine wait.");
                return;
            }

            var negotiatorPairs = result.Where(s => s.UserInfo.Cookie == cookie);

            var negotiators = negotiatorPairs.Where(s => s.UserInfo.ClientIndex == 0);


            var negotiatees = negotiatorPairs.Where(s => s.UserInfo.ClientIndex == 1);

            if (negotiators.Count() < 1 || negotiatees.Count() < 1)
            {
                return;
            }
            var negotiator = negotiators.First();
            var negotiatee = negotiatees.First();

            LogWriter.ToLog(LogEventLevel.Debug, $"Find negotiator {negotiator.RemoteEndPoint}");
            LogWriter.ToLog(LogEventLevel.Debug, $"Find negotiatee {negotiatee.RemoteEndPoint}");

            ConnectPacket packetToNegotiator = new ConnectPacket();
            byte[] dataToNegotiator = packetToNegotiator
                .SetVersionAndCookie(version, cookie)
                .SetRemoteEndPoint(negotiatee.RemoteEndPoint)
                .BuildResponse();

            ConnectPacket packetToNegotiatee = new ConnectPacket();
            byte[] dataToNegotiatee = packetToNegotiatee
                .SetVersionAndCookie(version, cookie)
                .SetRemoteEndPoint(negotiator.RemoteEndPoint)
                .BuildResponse();

            negotiator.UserInfo.UpdateRetryNatNegotiationTime();
            negotiatee.UserInfo.UpdateRetryNatNegotiationTime();

            negotiator.SendAsync(dataToNegotiator);
            negotiatee.SendAsync(dataToNegotiatee);

        }
    }
}
