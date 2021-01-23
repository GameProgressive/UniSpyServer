using QueryReport.Entity.Structure;
using ServerBrowser.Abstraction.BaseClass;

namespace ServerBrowser.Entity.Structure.Result
{
    internal sealed class ServerInfoResult : SBResultBase
    {
        public GameServerInfo GameServerInfo { get; set; }
        public ServerInfoResult()
        {
        }
    }
}
