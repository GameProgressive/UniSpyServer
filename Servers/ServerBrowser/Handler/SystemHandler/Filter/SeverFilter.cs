using System;
using System.Collections.Generic;
using System.Net;
using QueryReport.Entity.Structure;

namespace ServerBrowser.Handler.CommandHandler.ServerList.GetServers.Filter
{
    public class ServerFilter
    {
        public static IEnumerable<KeyValuePair<EndPoint, GameServer>> GetFilteredServer(IEnumerable<KeyValuePair<EndPoint, GameServer>> rawServer, string filter)
        {
            //TODO
            //We filter server for next step
            return rawServer;
        }
    }
}
