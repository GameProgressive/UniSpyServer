using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using QueryReport.Entity.Structure;
using ServerBrowser.Entity.Interface;

namespace ServerBrowser.Handler.CommandHandler.ServerList.GetServers
{
    public class GetServersFromMemory:IGetServerAble
    {
        public IEnumerable<KeyValuePair<EndPoint, GameServer>> GetOnlineServers(string gameName)
        {
           return QueryReport.Server.QRServer.GameServerList.
              Where(c => c.Value.ServerKeyValue.Data["gamename"]
              == gameName);
        }
    }
}
