using QueryReport.Entity.Structure;
using ServerBrowser.Abstraction.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
