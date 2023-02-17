namespace UniSpy.LinqToRedis
{
    public interface IRedisKey
    {
        string SearchKey { get; }
        string FullKey { get; }
    }
}