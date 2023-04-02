using UniSpy.Server.Core.Extension.Redis;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V2.Aggregate.Redis.GameServer;

namespace UniSpy.Server.QueryReport.V2.Aggregate.Redis
{
    public class HeartbeatChannel : RedisChannelBase<GameServerInfo>
    {
        public HeartbeatChannel() : base(RedisChannelName.HeartbeatChannel)
        {
        }
    }
}