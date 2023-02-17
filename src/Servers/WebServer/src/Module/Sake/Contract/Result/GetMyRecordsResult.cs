using System.Collections.Generic;
using UniSpy.Server.WebServer.Abstraction;
using UniSpy.Server.WebServer.Contract;

namespace UniSpy.Server.WebServer.Module.Sake.Contract.Result
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