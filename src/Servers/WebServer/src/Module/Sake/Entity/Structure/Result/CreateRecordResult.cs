using System.Collections.Generic;
using UniSpyServer.Servers.WebServer.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Sake.Structure.Result
{
    public sealed class CreateRecordResult : ResultBase
    {
        public string TableID { get; init; }
        public string RecordID { get; init; }
        public List<string> Fields { get; private set; }
        public CreateRecordResult()
        {
            Fields = new List<string>();
        }
    }
}