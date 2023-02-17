using System.Collections.Generic;
using UniSpy.Server.WebServer.Abstraction;
using UniSpy.Server.WebServer.Entity.Structure;

namespace UniSpy.Server.WebServer.Module.Sake.Structure.Result
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