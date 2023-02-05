using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass
{
    public abstract class RedisChannelBase<T>
    {
        protected string _redisChannelName;
        /// <summary>
        /// Get all subscriber in Redis
        /// </summary>
        protected ISubscriber _subscriber => ConfigManager.Config.Redis.RedisConnection.GetSubscriber();
        public RedisChannelBase(string redisChannelName)
        {
            _redisChannelName = redisChannelName;
        }

        /// <summary>
        /// Run StartSubscribe manually
        /// Some server only require publish message
        /// so they do not need to run this function
        /// </summary>
        public void StartSubscribe()
        {
            _subscriber.Subscribe(_redisChannelName, (channel, message)
                =>
            {
                T msg = DeserializeMessage(message);
                ReceivedMessage(msg);
            });
        }

        public virtual void ReceivedMessage(T message)
        {

        }

        public virtual void PublishMessage(T message)
        {
            if (message is null)
            {
                return;
            }
            string jsonStr = SerializeMessage(message);
            _subscriber.Publish(_redisChannelName, jsonStr);
        }

        protected virtual string SerializeMessage(T message)
        {
            return JsonConvert.SerializeObject(message);
        }

        protected virtual T DeserializeMessage(string message)
        {
            return JsonConvert.DeserializeObject<T>(message);
        }
    }
}