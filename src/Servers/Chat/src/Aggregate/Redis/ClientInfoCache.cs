using System;
using UniSpy.LinqToRedis;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Core.Extension.Redis;

namespace UniSpy.Server.Chat.Aggregate.Redis
{
    public record ClientInfoCache : Core.Abstraction.BaseClass.RedisKeyValueObject
    {
        [RedisKey]
        public string NickName { get; set; }
        public ClientInfo Info { get; set; }
        public ClientInfoCache() : base(RedisDbNumber.ChatChannel, TimeSpan.FromMinutes(2)) { }
        public class RedisClient : Core.Abstraction.BaseClass.RedisClient<ClientInfoCache>
        {
            public RedisClient() { }
        }
    }
}