using System;
using System.Collections.Generic;
using System.Net;
using QueryReport.Entity.Structure;

namespace ServerBrowser.Entity.Interface
{
    /// <summary>
    /// We do not want to care about which method system
    /// using to get the servers' information, we just need the data.
    /// </summary>
    public interface IGetServerAble
    {
        /// <summary>
        /// Get online servers by game name
        /// </summary>
        /// <param name="serverName"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<EndPoint, GameServer>> GetOnlineServers(string serverName);
    }
}
