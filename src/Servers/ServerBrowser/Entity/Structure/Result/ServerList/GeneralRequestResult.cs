using QueryReport.Entity.Structure;
using ServerBrowser.Abstraction.BaseClass;
using System.Collections.Generic;

namespace ServerBrowser.Entity.Structure.Result
{
    internal sealed class GeneralRequestResult : ServerListResultBase
    {
        public List<GameServerInfo> GameServerInfos { get; set; }
        public GeneralRequestResult()
        {
            GameServerInfos = new List<GameServerInfo>();
        }
    }
}
