using UniSpy.Server.QueryReport.V2.Aggregate.Redis.GameServer;
using UniSpy.Server.ServerBrowser.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.Contract.Result
{
    public sealed class ServerInfoResult : ServerListUpdateOptionResultBase
    {
        public GameServerInfo GameServerInfo { get; set; }
        public ServerInfoResult()
        {
        }
    }
}
