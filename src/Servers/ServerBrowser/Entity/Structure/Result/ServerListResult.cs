using System;
using QueryReport.Entity.Structure;
using ServerBrowser.Abstraction.BaseClass;
using System.Collections.Generic;
namespace ServerBrowser.Entity.Structure.Result
{
    public class ServerListResult : SBResultBase
    {
        public List<GameServerInfo> GamerServerInfos { get; set; }
        public ServerListResult()
        {
            GamerServerInfos = new List<GameServerInfo>();
        }
    }
}
