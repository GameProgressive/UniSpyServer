﻿using GameSpyLib.Logging;
using NatNegotiation.Entity.Enumerate;
using NatNegotiation.Entity.Structure.Packet;
using NatNegotiation.Server;
using Serilog.Events;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace NatNegotiation.Abstraction.SystemHandler.NatNegotiationManage
{
    public class NatNegotiationManager
    {
        public static ConcurrentDictionary<string, NatNegSession> SessionPool;

        static NatNegotiationManager()
        {
            SessionPool = new ConcurrentDictionary<string, NatNegSession>();
        }

        public static void Negotiate(NatPortType portType, byte version, uint cookie)
        {
            //find Sessions according to the partern
            Dictionary<string, NatNegSession> result = SessionPool
                 .Where(s => s.Key.Contains(portType.ToString()))
                  .ToDictionary(s => s.Key, s => s.Value);

            //there are at least a pair of session
            if (result.Count < 2)
            {
                LogWriter.ToLog(LogEventLevel.Debug, "No match found we continue waitting.");
                return;
            }

            //find the negotiator pairs
            var negotiatorPairs = result.Where(s => s.Value.UserInfo.Cookie == cookie);

            //find negitiators and negotiatees by a same cookie
            var negotiators = negotiatorPairs.Where(s => s.Value.UserInfo.ClientIndex == 0);
            var negotiatees = negotiatorPairs.Where(s => s.Value.UserInfo.ClientIndex == 1);

            //negotiator are not ready, we keep waitting
            if (negotiators.Count() < 1 || negotiatees.Count() < 1)
            {
                return;
            }

            //we find both negotiators
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

            //finally we remove two sessions from our session pool
            SessionPool.TryRemove(negotiatee.Key, out _);
            SessionPool.TryRemove(negotiator.Key, out _);

        }
    }
}
