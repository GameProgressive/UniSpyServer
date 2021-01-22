using QueryReport.Entity.Structure;
using ServerBrowser.Abstraction.BaseClass;
using System.Collections.Generic;

namespace ServerBrowser.Entity.Structure.Result
{
    internal sealed class GeneralRequestResult : UpdateOptionResultBase
    {
        public List<GameServerInfo> GamerServerInfos { get; set; }

        public GeneralRequestResult()
        {
            GamerServerInfos = new List<GameServerInfo>();
        }
    }
}
