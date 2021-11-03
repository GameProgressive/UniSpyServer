using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis;
using UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Structure.Result
{
    public sealed class ServerInfoResult : ServerListUpdateOptionResultBase
    {
        public GameServerInfo GameServerInfo { get; set; }
        public ServerInfoResult()
        {
        }
    }
}
