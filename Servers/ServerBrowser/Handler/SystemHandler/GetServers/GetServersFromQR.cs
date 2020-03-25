using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using QueryReport.Entity.Structure;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Interface;
using ServerBrowser.Entity.Structure;
using ServerBrowser.Entity.Structure.Packet.Request;
using ServerBrowser.Handler.CommandHandler.ServerList.GetServers.Filter;

namespace ServerBrowser.Handler.CommandHandler.ServerList.GetServers
{
    public class GetServersFromQR
    {
        public static IEnumerable<KeyValuePair<EndPoint, GameServer>> GetFilteredServer(IGetServerable iServer, string gameName, string filter)
        {
            return ServerFilter.GetFilteredServer(iServer.GetOnlineServers(gameName), filter);
        }
    }
}
