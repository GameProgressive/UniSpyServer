using UniSpy.Server.Core.Config;

namespace UniSpy.Server.Core.Abstraction.BaseClass
{
    public class RedisClient<TValue> : LinqToRedis.RedisClient<TValue> where TValue : RedisKeyValueObject
    {
        public RedisClient() : base(ConfigManager.Config.Redis.RedisConnection)
        {
        }
    }
}