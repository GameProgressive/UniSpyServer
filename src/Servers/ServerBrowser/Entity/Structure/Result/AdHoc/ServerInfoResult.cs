using QueryReport.Entity.Structure.Redis;
using ServerBrowser.Abstraction.BaseClass;

namespace ServerBrowser.Entity.Structure.Result
{
    internal sealed class ServerInfoResult : ServerListUpdateOptionResultBase
    {
        public GameServerInfo GameServerInfo { get; set; }
        public ServerInfoResult()
        {
        }
    }
}
