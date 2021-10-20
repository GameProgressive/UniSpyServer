using UniSpyServer.QueryReport.Entity.Structure.Redis;
using UniSpyServer.ServerBrowser.Abstraction.BaseClass;

namespace UniSpyServer.ServerBrowser.Entity.Structure.Result
{
    public sealed class P2PGroupRoomListResult : ServerListUpdateOptionResultBase
    {
        public PeerGroupInfo PeerGroupInfo { get; set; }
        public P2PGroupRoomListResult()
        {
        }
    }
}
