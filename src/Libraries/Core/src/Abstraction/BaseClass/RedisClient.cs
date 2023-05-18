using StackExchange.Redis;

namespace UniSpy.Server.Core.Abstraction.BaseClass
{
    public class RedisClient<TValue> : LinqToRedis.RedisClient<TValue> where TValue : RedisKeyValueObject
    {
        public RedisClient(IConnectionMultiplexer multiplexer) : base(multiplexer)
        {
        }
    }
}