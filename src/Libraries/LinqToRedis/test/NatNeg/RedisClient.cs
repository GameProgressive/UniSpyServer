namespace UniSpy.Redis.Test
{
    internal class RedisClient : UniSpyServer.LinqToRedis.RedisClient<UserInfo>
    {
        public RedisClient() : base("101.34.72.231:6666", 0)
        {
        }
    }
}