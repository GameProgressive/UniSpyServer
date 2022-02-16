using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass.Redis
{
    public abstract class RedisChannel<T>
    {
        protected string _redisChannelName;
        /// <summary>
        /// Get all subscriber in Redis
        /// </summary>
        protected ISubscriber _subscriber => ClientBase.Redis.GetSubscriber();
        public RedisChannel(string redisChannelName)
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
            if (message == null)
            {
                return;
            }
            string jsonStr = SerializeMessage(message);
            _subscriber.Publish(_redisChannelName, jsonStr);
        }

        public virtual string SerializeMessage(T message)
        {
            return JsonConvert.SerializeObject(message);
        }

        public virtual T DeserializeMessage(string message)
        {
            return JsonConvert.DeserializeObject<T>(message);
        }
    }
}
