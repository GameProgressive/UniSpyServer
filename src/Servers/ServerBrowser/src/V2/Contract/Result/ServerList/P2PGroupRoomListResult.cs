using System.Collections.Generic;
using UniSpy.Server.QueryReport.Aggregate.Redis.PeerGroup;
using UniSpy.Server.ServerBrowser.V2.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.V2.Contract.Result
{
    public sealed class P2PGroupRoomListResult : ServerListUpdateOptionResultBase
    {
        public List<PeerRoomInfo> PeerRoomsInfo { get; set; }
        public P2PGroupRoomListResult()
        {
        }
    }
}
