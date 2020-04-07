using QueryReport.Entity.Structure;
using System.Collections.Generic;
using System.Net;

namespace ServerBrowser.Entity.Interface
{
    /// <summary>
    /// We do not want to care about which method system
    /// using to get the servers' information, we just need the data.
    /// </summary>
    public interface IGetServerable
    {
        /// <summary>
        /// Get online servers by game name
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<EndPoint,  GameServer>> GetOnlineServers(string gameName);
    }
}
