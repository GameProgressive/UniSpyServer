using System;
using System.Collections.Generic;
using System.Net;
using QueryReport.Entity.Structure;
using ServerBrowser.Entity.Interface;

namespace ServerBrowser.Handler.CommandHandler.ServerInfo.GetServer
{
    public class GetServerFromNetwork:IGetServerable
    {
        public GetServerFromNetwork()
        {
        }

        public IEnumerable<KeyValuePair<EndPoint, GameServer>> GetOnlineServers(string serverName)
        {
            throw new NotImplementedException();
        }
    }
}
