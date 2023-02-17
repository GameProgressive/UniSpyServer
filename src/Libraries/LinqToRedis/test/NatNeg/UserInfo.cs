using System;
using UniSpy.LinqToRedis;

namespace UniSpy.Redis.Test
{
    public record UserInfo : RedisKeyValueObject
    {
        [RedisKey]
        public Guid? ServerID { get; set; }
        [RedisKey]
        public int? Cookie { get; set; }
        [RedisKey]
        public string RemoteEndPoint { get; set; }
        public string UserName { get; set; }
        public UserInfo() : base(TimeSpan.FromMinutes(3))
        {
            // we set the expire time to 3 minutes
        }

    }
}