using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSpyServer.Servers.WebServer.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.D2G.Entity.Result
{
    public class GetStoreAvailabilityResult : ResultBase
    {
        public int Status { get; set; }
        public int StoreResult { get; set; }

        public GetStoreAvailabilityResult()
        {

        }
    }
}
