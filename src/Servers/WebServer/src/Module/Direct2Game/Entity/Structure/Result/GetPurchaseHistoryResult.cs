using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSpyServer.Servers.WebServer.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Direct2Game.Entity.Structure.Result
{
    public class GetPurchaseHistoryResult : ResultBase
    {
        public int Status { get; set; }
    }
}
