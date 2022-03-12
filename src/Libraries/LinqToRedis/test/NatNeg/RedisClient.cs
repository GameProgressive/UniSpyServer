namespace UniSpy.Redis.Test
{
    internal class RedisClient : UniSpyServer.LinqToRedis.RedisClient<UserInfo>
    {
        public RedisClient() : base("127.0.0.1:6379", 0)
        {
        }
    }
}