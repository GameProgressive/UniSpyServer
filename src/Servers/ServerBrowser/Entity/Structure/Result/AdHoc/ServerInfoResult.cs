using QueryReport.Entity.Structure.Redis;
using ServerBrowser.Abstraction.BaseClass;

namespace ServerBrowser.Entity.Structure.Result
{
    internal sealed class ServerInfoResult : ServerListResultBase
    {
        public GameServerInfo GameServerInfo { get; set; }
        public ServerInfoResult()
        {
        }
    }
}
