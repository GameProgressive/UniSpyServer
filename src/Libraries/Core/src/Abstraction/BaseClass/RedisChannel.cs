using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpy.Server.Core.Config;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.Core.Abstraction.BaseClass
{

    public abstract class RedisChannelBase<T>
    {
        public delegate void OnReceivedMessageEventHandler(T message);
        public event OnReceivedMessageEventHandler OnReceived;
        protected string _redisChannelName;
        /// <summary>
        /// Get all subscriber in Redis
        /// </summary>
        protected ISubscriber _subscriber = ConfigManager.Config.Redis.RedisConnection.GetSubscriber();
        public bool IsStarted { get; private set; }
        public RedisChannelBase(string redisChannelName)
        {
            _redisChannelName = redisChannelName;
            OnReceived += ReceivedMessage;
        }

        /// <summary>
        /// Run StartSubscribe manually
        /// Some server only require publish message
        /// so they do not need to run this function
        /// </summary>
        public void StartSubscribe()
        {
            if (IsStarted == false)
            {
                _subscriber.Subscribe(_redisChannelName, (channel, message)
                    =>
                {
                    T msg = DeserializeMessage(message);
                    OnReceived(msg);
                });
                IsStarted = true;
            }
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
            try
            {
                _subscriber.Publish(_redisChannelName, jsonStr);
            }
            catch (StackExchange.Redis.RedisTimeoutException ex)
            {
                if (!System.Diagnostics.Debugger.IsAttached)
                {
                    throw ex;
                }
                else
                {
                    LogWriter.LogWarn("Redis channel timeout because of break point.");
                }
            }
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
