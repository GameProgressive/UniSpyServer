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
            //find Sessions according to the partern
            Dictionary<string, NatNegSession> result = Sessions
                 .Where(s => s.Key.Contains(portType.ToString()))
                  .ToDictionary(s => s.Key, s => s.Value);

            //there are at least a pair of session
            if (result.Count < 2)
            {
                LogWriter.ToLog(LogEventLevel.Debug, "No match found we contine wait.");
                return;
            }

            //find the pairs
            var negotiatorPairs = result.Where(s => s.Value.UserInfo.Cookie == cookie);

            //find negitiators and negotiatees by cookie
            var negotiators = negotiatorPairs.Where(s => s.Value.UserInfo.ClientIndex == 0);
            var negotiatees = negotiatorPairs.Where(s => s.Value.UserInfo.ClientIndex == 1);

            if (negotiators.Count() < 1 || negotiatees.Count() < 1)
            {
                return;
            }
            var negotiator = negotiators.First();
            var negotiatee = negotiatees.First();

            LogWriter.ToLog(LogEventLevel.Debug, $"Find negotiator {negotiator.Value.RemoteEndPoint}");
            LogWriter.ToLog(LogEventLevel.Debug, $"Find negotiatee {negotiatee.Value.RemoteEndPoint}");

            ConnectPacket packetToNegotiator = new ConnectPacket();
            byte[] dataToNegotiator = packetToNegotiator
                .SetVersionAndCookie(version, cookie)
                .SetRemoteEndPoint(negotiatee.Value.RemoteEndPoint)
                .BuildResponse();

            ConnectPacket packetToNegotiatee = new ConnectPacket();
            byte[] dataToNegotiatee = packetToNegotiatee
                .SetVersionAndCookie(version, cookie)
                .SetRemoteEndPoint(negotiator.Value.RemoteEndPoint)
                .BuildResponse();

            negotiator.Value.UserInfo.UpdateRetryNatNegotiationTime();
            negotiatee.Value.UserInfo.UpdateRetryNatNegotiationTime();

            negotiator.Value.SendAsync(dataToNegotiator);
            negotiatee.Value.SendAsync(dataToNegotiatee);

            Sessions.TryRemove(negotiatee.Key, out _);
            Sessions.TryRemove(negotiator.Key, out _);

        }
    }
}
