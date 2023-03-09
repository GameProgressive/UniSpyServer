using UniSpy.Server.Core.Extension.Redis;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.Aggregate.Redis.GameServer;

namespace UniSpy.Server.QueryReport.Aggregate.Redis
{
    public class HeartbeatChannel : RedisChannelBase<GameServerInfo>
    {
        public HeartbeatChannel() : base(RedisChannelName.HeartbeatChannel)
        {
        }
    }
}