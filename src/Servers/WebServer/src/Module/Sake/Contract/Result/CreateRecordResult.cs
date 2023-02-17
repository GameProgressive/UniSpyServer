using System.Collections.Generic;
using UniSpy.Server.WebServer.Module.Sake.Abstraction;

namespace UniSpy.Server.WebServer.Module.Sake.Contract.Result
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