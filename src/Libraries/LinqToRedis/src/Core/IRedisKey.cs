namespace UniSpyServer.LinqToRedis
{
    public interface IRedisKey
    {
        string SearchKey { get; }
        string FullKey { get; }
    }
}