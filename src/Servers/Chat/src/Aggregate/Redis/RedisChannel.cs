using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.Chat.Aggregate.Redis
{
    /// <summary>
    /// When a local channel is created the user message will send to redis channel
    /// redis channel is like a broadcast platform which will broadcast the message to all the user
    /// when user is connected to unispy chat server
    /// </summary>
    public class RedisChannel : RedisChannelBase<string>
    {
        public RedisChannel(string redisChannelName) : base(redisChannelName){ }
    }
}