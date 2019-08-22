using GameSpyLib.Logging;
using GameSpyLib.Network;
using QueryReport.GameServerInfo;
using System;
using System.Collections.Generic;

namespace QueryReport.Handler
{
    /// <summary>
    /// This class contians gamespy master udp server functions  which help cdkeyserver to finish the master tcp server functionality. 
    /// This class is used to simplify the functions in server class, separate the other utility function making  the main server logic clearer
    /// </summary>
    public class QRHandler
   {
        public static QRDBQuery DBQuery = null;

        public static void UpdateServerOffline(GameServer server)
        {
            DBQuery.UpdateServerOffline(server);
        }

        public static string HeartBeatChallengeGen()
        {
            return "";
        }

    }
}
