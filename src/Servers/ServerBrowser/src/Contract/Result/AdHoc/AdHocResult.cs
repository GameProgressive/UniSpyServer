using UniSpy.Server.QueryReport.Aggregate.Redis.GameServer;
using UniSpy.Server.ServerBrowser.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.Contract.Result
{
    public sealed class AdHocResult : ResultBase
    {
        public GameServerInfo GameServerInfo { get; set; }
        public AdHocResult()
        {
        }
    }
}
