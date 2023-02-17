using UniSpy.Server.QueryReport.V2.Aggregate.Redis.GameServer;
using UniSpy.Server.ServerBrowser.V2.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.V2.Contract.Result
{
    public sealed class ServerInfoResult : ServerListUpdateOptionResultBase
    {
        public GameServerInfo GameServerInfo { get; set; }
        public ServerInfoResult()
        {
        }
    }
}
