using System.Collections.Generic;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Structure;

namespace UniSpyServer.Servers.WebServer.Module.Sake.Structure.Result
{
    public class GetMyRecordsResult : ResultBase
    {
        public List<RecordFieldObject> Records { get; private set; }
        public GetMyRecordsResult()
        {
            Records = new List<RecordFieldObject>();
        }
    }
}