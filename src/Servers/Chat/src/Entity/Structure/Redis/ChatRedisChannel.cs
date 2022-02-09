using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Redis;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Redis
{
    /// <summary>
    /// When a local channel is created the user message will send to redis channel
    /// redis channel is like a broadcast platform which will broadcast the message to all the user
    /// when user is connected to unispy chat server
    /// </summary>
    public class ChatRedisChannel : RedisChannel<string>
    {
        public ChatRedisChannel(string redisChannelName) : base(redisChannelName)
        {
        }
    }
}