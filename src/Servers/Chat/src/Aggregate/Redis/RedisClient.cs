using UniSpy.Server.Core.Config;
using UniSpy.Server.Core.Extension.Redis;

namespace UniSpy.Server.Chat.Aggregate.Redis
{
    public class RedisClient : LinqToRedis.RedisClient<Channel>
    {
        public RedisClient() : base(ConfigManager.Config.Redis.RedisConnection, (int)RedisDbNumber.ChatChannel, true)
        {
        }
    }
}