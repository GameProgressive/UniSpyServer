using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Redis.GameServer;
using UniSpyServer.Servers.ServerBrowser.V2.Abstraction.BaseClass;

namespace UniSpyServer.Servers.ServerBrowser.V2.Entity.Structure.Result
{
    public sealed class ServerInfoResult : ServerListUpdateOptionResultBase
    {
        public GameServerInfo GameServerInfo { get; set; }
        public ServerInfoResult()
        {
        }
    }
}
