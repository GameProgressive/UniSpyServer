using System;

namespace UniSpyServer.LinqToRedis
{
    /// <summary>
    /// Mark property as redis key attribute, so you can search by this key
    /// The key must be simple object, which mean its ToString method can out put a valid string
    /// Such as int, string, DateTime, Guid, IPEndPoint, etc.
    /// </summary>
    public class RedisKeyAttribute : Attribute
    {
        public RedisKeyAttribute() { }
    }
}