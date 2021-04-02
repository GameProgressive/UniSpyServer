using QueryReport.Entity.Structure.Redis;
using ServerBrowser.Abstraction.BaseClass;

namespace ServerBrowser.Entity.Structure.Result
{
    internal sealed class P2PGroupRoomListResult : ServerListUpdateOptionResultBase
    {
        public PeerGroupInfo PeerGroupInfo { get; set; }
        public P2PGroupRoomListResult()
        {
        }
    }
}
