using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Redis.PeerGroup;
using UniSpyServer.Servers.ServerBrowser.V2.Abstraction.BaseClass;

namespace UniSpyServer.Servers.ServerBrowser.V2.Entity.Structure.Result
{
    public sealed class P2PGroupRoomListResult : ServerListUpdateOptionResultBase
    {
        public PeerGroupInfo PeerGroupInfo { get; set; }
        public P2PGroupRoomListResult()
        {
        }
    }
}
