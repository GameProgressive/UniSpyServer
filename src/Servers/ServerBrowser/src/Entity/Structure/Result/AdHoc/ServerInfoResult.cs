using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.GameServer;
using UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Structure.Result
{
    public sealed class ServerInfoResult : ServerListUpdateOptionResultBase
    {
        public GameServerInfo2 GameServerInfo { get; set; }
        public ServerInfoResult()
        {
        }
    }
}
