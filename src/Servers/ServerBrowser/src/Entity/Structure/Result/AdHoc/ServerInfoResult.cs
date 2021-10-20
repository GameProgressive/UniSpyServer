using UniSpyServer.QueryReport.Entity.Structure.Redis;
using UniSpyServer.ServerBrowser.Abstraction.BaseClass;

namespace UniSpyServer.ServerBrowser.Entity.Structure.Result
{
    public sealed class ServerInfoResult : ServerListUpdateOptionResultBase
    {
        public GameServerInfo GameServerInfo { get; set; }
        public ServerInfoResult()
        {
        }
    }
}
