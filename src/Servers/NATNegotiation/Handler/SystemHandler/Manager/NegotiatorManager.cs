using System.Collections.Generic;
using System.Net;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure;
using UniSpyLib.Extensions;

namespace NATNegotiation.Handler.SystemHandler.Manager
{
    public class NegotiatorManager
    {
        //public static ConcurrentDictionary<string, NNSession> SessionPool = new ConcurrentDictionary<string, NNSession>();

        public static void Negotiate(NatPortType portType, byte version, uint cookie)
        {
            ////find Sessions according to the partern
            //Dictionary<string, NNSession> result = SessionPool
            //     .Where(s => s.Key.Contains(portType.ToString()))
            //      .ToDictionary(s => s.Key, s => s.Value);

            ////there are at least a pair of session
            //if (result.Count < 2)
            //{
            //    LogWriter.ToLog(LogEventLevel.Debug, "No match found we continue waitting.");
            //    return;
            //}

            ////find the negotiator pairs
            //var negotiatorPairs = result.Where(s => s.Value.UserInfo.Cookie == cookie);

            ////find negitiators and negotiatees by a same cookie
            //var negotiators = negotiatorPairs.Where(s => s.Value.UserInfo.ClientIndex == 0);
            //var negotiatees = negotiatorPairs.Where(s => s.Value.UserInfo.ClientIndex == 1);

            ////negotiator are not ready, we keep waitting
            //if (negotiators.Count() < 1 || negotiatees.Count() < 1)
            //{
            //    return;
            //}

            ////we find both negotiators
            //var negotiator = negotiators.First();
            //var negotiatee = negotiatees.First();

            //LogWriter.ToLog(LogEventLevel.Debug, $"Find negotiator {negotiator.Value.RemoteEndPoint}");
            //LogWriter.ToLog(LogEventLevel.Debug, $"Find negotiatee {negotiatee.Value.RemoteEndPoint}");

            //byte[] dataToNegotiator =
            //    new ConnectResponse(version, cookie, negotiatee.Value.RemoteEndPoint).BuildResponse();

            //byte[] dataToNegotiatee =
            //    new ConnectResponse(version, cookie, negotiator.Value.RemoteEndPoint).BuildResponse();

            //negotiator.Value.UserInfo.UpdateRetryNATNegotiationTime();
            //negotiatee.Value.UserInfo.UpdateRetryNATNegotiationTime();

            //negotiator.Value.SendAsync(dataToNegotiator);
            //negotiatee.Value.SendAsync(dataToNegotiatee);


            //NNServerManager.Server.SendAsync(EndPoint, dataToNegotiator);
            //NNServerManager.Server.SendAsync(EndPoint, dataToNegotiatee);
            ////finally we remove two sessions from our session pool
            //SessionPool.TryRemove(negotiatee.Key, out _);
            //SessionPool.TryRemove(negotiator.Key, out _);
        }


    }
}
