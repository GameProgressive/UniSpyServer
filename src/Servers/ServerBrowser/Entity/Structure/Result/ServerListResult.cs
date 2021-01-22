using QueryReport.Entity.Structure;
using ServerBrowser.Abstraction.BaseClass;
using System.Collections.Generic;
namespace ServerBrowser.Entity.Structure.Result
{
    internal sealed class ServerListResult : UpdateOptionResultBase
    {
        public List<GameServerInfo> GamerServerInfos { get; set; }
        public ServerListResult()
        {
            GamerServerInfos = new List<GameServerInfo>();
        }
    }
}
