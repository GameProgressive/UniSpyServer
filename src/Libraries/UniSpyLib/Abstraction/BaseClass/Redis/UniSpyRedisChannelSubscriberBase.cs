using StackExchange.Redis;

namespace UniSpyLib.Abstraction.BaseClass.Redis
{
    public abstract class UniSpyRedisChannelSubscriberBase<T>
    {
        protected string _redisChannelName;
        /// <summary>
        /// Get all subscriber in Redis
        /// </summary>
        protected ISubscriber _subscriber => UniSpyServerFactoryBase.Redis.GetSubscriber();
        public UniSpyRedisChannelSubscriberBase(string redisChannelName)
        {
            _redisChannelName = redisChannelName;
        }

        /// <summary>
        /// Run StartSubscribe manually
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
            return default;
        }

        public virtual T DeserializeMessage(string message)
        {
            return default;
        }
    }
}
