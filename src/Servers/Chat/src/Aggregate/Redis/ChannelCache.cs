using System;
using UniSpy.LinqToRedis;
using UniSpy.Server.Core.Extension.Redis;

namespace UniSpy.Server.Chat.Aggregate.Redis
{
    public record ChannelCache : Core.Abstraction.BaseClass.RedisKeyValueObject
    {
        [RedisKey]
        public string GameName { get; set; }
        [RedisKey]
        public string ChannelName { get; set; }
        // [RedisKey]
        // public string PreviousJoinedChannel => Channel?.PreviousJoinedChannel;
        public Channel Channel { get; set; }
        public ChannelCache() : base(RedisDbNumber.ChatChannel, TimeSpan.FromMinutes(1))
        {
        }
        public class RedisClient : Core.Abstraction.BaseClass.RedisClient<ChannelCache>
        {
            public RedisClient() { }
        }
    }

}