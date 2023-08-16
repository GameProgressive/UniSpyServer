using System.Collections.Generic;
using UniSpy.Server.WebServer.Module.Sake.Abstraction;

namespace UniSpy.Server.WebServer.Module.Sake.Contract.Result
{
    public class SearchForRecordsResult : ResultBase
    {
        public Dictionary<string, string> UserData;
    }
}