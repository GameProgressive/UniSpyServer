using System;
using System.Collections.Generic;
using System.Net;
using QueryReport.Entity.Structure;

namespace ServerBrowser.Handler.CommandHandler.ServerList.GetServers.Filter
{
    public class ServerFilter
    {
        IEnumerable<KeyValuePair<EndPoint, GameServer>> _rawServer;
        string _filter;
        public ServerFilter(IEnumerable<KeyValuePair<EndPoint, GameServer>> rawServer, string filter )
        {
            _rawServer = rawServer;
            _filter = filter;
        }
        public IEnumerable<KeyValuePair<EndPoint, GameServer>> GetFilteredServer()
        {
            //TODO
            //We filter server for next step
            return _rawServer;
        }
    }
}
