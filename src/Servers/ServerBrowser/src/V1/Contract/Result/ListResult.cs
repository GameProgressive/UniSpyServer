using System.Collections.Generic;
using System.Net;
using UniSpy.Server.QueryReport.V1.Aggregation.Redis;
using UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.V1.Contract.Result
{
    public sealed class ListResult : ResultBase
    {
        public List<GameServerInfo> ServersInfo { get; set; }
        public ListResult()
        {
        }
    }
}