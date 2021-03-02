using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Redis;
using ServerBrowser.Abstraction.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerBrowser.Entity.Structure.Result.ServerList
{

    internal sealed class NoServerListResult : ServerListResultBase
    {
        public List<GameServerInfo> GameServerInfos { get; set; }
        public NoServerListResult()
        {
            GameServerInfos = new List<GameServerInfo>();
        }
    }
}
