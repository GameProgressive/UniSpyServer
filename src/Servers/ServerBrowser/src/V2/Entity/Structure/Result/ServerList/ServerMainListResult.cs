using UniSpy.Server.ServerBrowser.V2.Abstraction.BaseClass;
using System.Collections.Generic;
using UniSpy.Server.QueryReport.V2.Entity.Structure.Redis.GameServer;

namespace UniSpy.Server.ServerBrowser.V2.Entity.Structure.Result
{
    public class ServerMainListResult : ServerListUpdateOptionResultBase
    {
        public List<GameServerInfo> GameServerInfos { get; set; }
        public ServerMainListResult()
        {
            GameServerInfos = new List<GameServerInfo>();
        }
    }
}
