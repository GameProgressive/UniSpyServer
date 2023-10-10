using System.Collections.Generic;
using UniSpy.Server.QueryReport.Aggregate.Redis.PeerGroup;
using UniSpy.Server.QueryReport.V1.Aggregation.Redis;
using UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.V1.Contract.Result
{
    public sealed class ListResult : ResultBase
    {
        public List<GameServerCache> ServersInfo { get; set; }
        public List<PeerRoomInfo> PeerRoomsInfo { get; set; }
        public ListResult()
        {
        }
    }
}