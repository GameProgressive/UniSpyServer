using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Entity.Structure.Result;
using NATNegotiation.Handler.SystemHandler.Redis;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UniSpyLib.Logging;

namespace NATNegotiation.Handler.SystemHandler.Manager
{
    public class NatNegotiateManager
    {
        public static void Negotiate(NatPortType portType, byte version, uint cookie)
        {
            var searchKey = NatUserInfoRedisOperator.BuildSearchKey(portType, cookie);

            List<string> matchedKeys = NatUserInfoRedisOperator.GetMatchedKeys(searchKey);
            // because cookie is unique for each client we will only get 2 of keys
            if (matchedKeys.Count != 2)
            {
                LogWriter.ToLog("No match found we continue waitting.");
                return;
            }

            Dictionary<string, NatUserInfo> negotiatorPairs = new Dictionary<string, NatUserInfo>();

            foreach (var key in matchedKeys)
            {
                negotiatorPairs.Add(key, NatUserInfoRedisOperator.GetSpecificValue(key));

                ////find negitiators and negotiatees by a same cookie
                var negotiators = negotiatorPairs.Where(s => s.Value.InitRequestInfo.ClientIndex == 0);
                var negotiatees = negotiatorPairs.Where(s => s.Value.InitRequestInfo.ClientIndex == 1);

                if (negotiators.Count() != 1 || negotiatees.Count() != 1)
                {
                    LogWriter.ToLog("No match found, we keep waiting!");
                    return;
                }

                // we only can find one pair of the users
                var negotiator = negotiators.First();
                var negotiatee = negotiatees.First();

                LogWriter.ToLog(LogEventLevel.Debug, $"Find negotiator {negotiator.Value.RemoteEndPoint}");
                LogWriter.ToLog(LogEventLevel.Debug, $"Find negotiatee {negotiatee.Value.RemoteEndPoint}");
                // exchange data for each other
                EndPoint endOfNegotiator = negotiator.Value.RemoteEndPoint;
                EndPoint endOfNegotiatee = negotiatee.Value.RemoteEndPoint;

                var result1 = new ConnectResult()
                {
                    RemoteEndPoint = negotiatee.Value.RemoteEndPoint,
                    Cookie = cookie
                };
                var result2 = new ConnectResult()
                {
                    RemoteEndPoint = negotiator.Value.RemoteEndPoint,
                    Cookie = cookie
                };
                //TODO
                throw new NotImplementedException();
                //byte[] dataToNegotiator =
                //    new ConnectResponse(version, cookie, endOfNegotiatee);
                //byte[] dataToNegotiatee =
                //    new ConnectResponse(version, cookie, endOfNegotiator);

                //NNServerManager.Server.SendAsync(endOfNegotiator, dataToNegotiator);
                //NNServerManager.Server.SendAsync(endOfNegotiatee, dataToNegotiatee);
            }
        }
    }
}