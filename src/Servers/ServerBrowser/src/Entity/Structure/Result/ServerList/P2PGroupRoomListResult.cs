using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis;
using UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Structure.Result
{
    public sealed class P2PGroupRoomListResult : ServerListUpdateOptionResultBase
    {
        public PeerGroupInfo PeerGroupInfo { get; set; }
        public P2PGroupRoomListResult()
        {
        }
    }
}
