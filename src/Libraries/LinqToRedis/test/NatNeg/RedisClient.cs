
namespace UniSpy.Redis.Test
{
    internal class RedisClient : UniSpy.LinqToRedis.RedisClient<UserInfo>
    {
        public RedisClient() : base("127.0.0.1:6379", 10)
        {
        }
    }
}