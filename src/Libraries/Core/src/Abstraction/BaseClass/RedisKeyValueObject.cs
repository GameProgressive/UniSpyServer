using System;
using UniSpy.Server.Core.Extension.Redis;

namespace UniSpy.Server.Core.Abstraction.BaseClass
{
    public record RedisKeyValueObject : LinqToRedis.RedisKeyValueObject
    {
        public RedisKeyValueObject()
        {
        }

        public RedisKeyValueObject(RedisDbNumber db, TimeSpan? expireTime = null) : base((int)db, expireTime)
        {
        }
    }
}