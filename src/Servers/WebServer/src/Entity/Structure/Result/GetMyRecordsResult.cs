using System.Collections.Generic;
using System.Xml.Serialization;
using UniSpyServer.Servers.WebServer.Abstraction;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Result
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