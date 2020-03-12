using System;
using System.Collections.Generic;
using System.Net;
using QueryReport.Entity.Structure;
using ServerBrowser.Entity.Interface;

namespace ServerBrowser.Handler.CommandHandler.ServerList.GetServers
{
    /// <summary>
    /// This class will get server list  from network
    /// </summary>
    public class GetServersFromNetwork:IGetServerAble
    {
        public IEnumerable<KeyValuePair<EndPoint, GameServer>> GetOnlineServers(string serverName)
        {
            throw new NotImplementedException();
        }
    }
}
