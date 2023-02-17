using UniSpy.Server.QueryReport.V2.Aggregate.Redis.PeerGroup;
using UniSpy.Server.ServerBrowser.V2.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.V2.Contract.Result
{
    public sealed class P2PGroupRoomListResult : ServerListUpdateOptionResultBase
    {
        public PeerGroupInfo PeerGroupInfo { get; set; }
        public P2PGroupRoomListResult()
        {
        }
    }
}
